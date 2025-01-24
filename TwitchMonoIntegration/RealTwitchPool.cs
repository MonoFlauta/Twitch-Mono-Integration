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
        private readonly ISubject<Dictionary<string, long>> _onResultFinish;

        public RealTwitchPool(GameTask<Poll> pool)
        {
            _pool = pool;
            _onResultFinish = new Subject<Dictionary<string, long>>();
            _pool.GetAwaiter().OnCompleted(() =>
            {
                _onResultFinish.OnNext(Results());
            });
        }

        public override Dictionary<string, long> Results() => 
            _pool.MaybeResult.Info.Choices.ToDictionary(x => x.Title, x => x.Votes);

        public override IObservable<Dictionary<string, long>> ResultsOnFinish() =>
            _onResultFinish;
    }
}