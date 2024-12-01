using System;
using System.Collections.Generic;

namespace TwitchMonoIntegration
{
    public abstract class TwitchPool
    {
        public abstract Dictionary<string, long> Results();
        public abstract IObservable<Dictionary<string, long>> ResultsOnFinish();
    }
}