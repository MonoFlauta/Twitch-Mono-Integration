using System;
using System.Collections.Generic;

namespace TwitchMonoIntegration
{
    public abstract class TwitchPoll
    {
        public abstract Dictionary<string, long> Results();
        public abstract IObservable<Dictionary<string, long>> ResultsOnFinish();
        public abstract IObservable<TwitchSDK.Poll> Start();
    }
}