using System;
using TwitchSDK;
using TwitchSDK.Interop;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class EditorTwitchService : TwitchService
    {
        private EditorTwitchTestView _testView;
        private int _lastViewCount;
        private int _lastInternalViewCount;
        
        public override void Initialize(MonoBehaviour monoBehaviour)
        {
            Initialized.Value = true;
            AuthStatus.Value = TwitchSDK.Interop.AuthStatus.Loading;
        }

        public override TwitchPool NewSimplePool(string title, string[] choices, long duration)
        {
            throw new NotImplementedException();
        }

        public override IObservable<long> ObserveViewerCount()
        {
            throw new NotImplementedException();
        }

        public override long LastViewerCount() => 
            _lastViewCount;

        public override void SyncAuthStatus()
        {
            Log("Sync status");
        }

        public override IObservable<StreamInfo> GetAndSyncStreamInfo() =>
            Observable.Return(new StreamInfo
                {
                    ViewerCount =  _lastInternalViewCount
                })
                .Do(x => _lastViewCount = (int)x.ViewerCount);

        public void SetViewerCountTo(int viewerCount)
        {
            _lastInternalViewCount = viewerCount;
        }

        private void Log(string message)
        {
            Debug.Log($"Editor Twitch Service: {message}");
        }
    }
}