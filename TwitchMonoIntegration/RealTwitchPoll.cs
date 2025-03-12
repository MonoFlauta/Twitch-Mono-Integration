using System;
using System.Collections.Generic;
using System.Linq;
using TwitchSDK;
using UniRx;

namespace TwitchMonoIntegration
{
    public class RealTwitchPoll : TwitchPoll
    {
        private readonly GameTask<Poll> _poll;
        private readonly ISubject<Dictionary<string, long>> _onResultFinish;

        public RealTwitchPoll(GameTask<Poll> poll)
        {
            _poll = poll;
            _onResultFinish = new Subject<Dictionary<string, long>>();
        }

        public override Dictionary<string, long> Results() => 
            _poll.MaybeResult.Info.Choices.ToDictionary(x => x.Title, x => x.Votes);

        public override IObservable<Dictionary<string, long>> ResultsOnFinish() =>
            _onResultFinish;

        public override IObservable<Poll> Start() => 
            _poll.Task.ToObservable()
                .DoOnSubscribe(() => _poll.GetAwaiter().OnCompleted(() => _onResultFinish.OnNext(Results())));
    }
}