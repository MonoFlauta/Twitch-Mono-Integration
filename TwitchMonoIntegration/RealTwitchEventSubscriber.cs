using System;
using TwitchSDK;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public abstract class RealTwitchEventSubscriber<T> : MonoBehaviour
    {
        protected GameTask<EventStream<T>> task;

        private bool _withLogs;
        private bool _initialized;
        private ISubject<T> _subject; 
        
        public IObservable<T> OnReceiveEvent()
        {
            if (!_initialized) Initialize();
            return _subject;
        }

        protected virtual void Initialize()
        {
            _subject = new Subject<T>();
            _initialized = true;
            if (_withLogs)
                _subject
                    .Subscribe(x => Debug.Log($"Twitch Event Received: {LogMessage(x)}"))
                    .AddTo(this);
        }

        private void Update()
        {
            if (!_initialized) return;

            task.MaybeResult.TryGetNextEvent(out var e);
            if(e != null)
                _subject.OnNext(e);
        }

        protected abstract string LogMessage(T e);

        public static T2 CreateInstance<T2>(bool logs = true) where T2 : RealTwitchEventSubscriber<T>
        {
            var go = new GameObject("Container" + typeof(T));
            DontDestroyOnLoad(go);
            var component = go.AddComponent<T2>();
            component._withLogs = logs;
            return component;
        }
    }
}