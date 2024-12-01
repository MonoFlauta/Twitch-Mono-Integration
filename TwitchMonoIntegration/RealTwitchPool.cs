using System;
using System.Collections.Generic;
using System.Linq;
using TwitchSDK;
using UniRx;

namespace TwitchMonoIntegration
{
    public class RealTwitchPool : TwitchPool
    {
        private readonly GameTask<Poll> _pool;

        public RealTwitchPool(GameTask<Poll> pool)
        {
            _pool = pool;
        }

        public override Dictionary<string, long> Results() => 
            _pool.MaybeResult.Info.Choices.ToDictionary(x => x.Title, x => x.Votes);

        public override IObservable<Dictionary<string, long>> ResultsOnFinish() =>
            _pool.MaybeResult.ObserveEveryValueChanged(x => x.PollEnded)
                .Where(x => x.IsCompleted)
                .Select(_ => Results());
    }
}