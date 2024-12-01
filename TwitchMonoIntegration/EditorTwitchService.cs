using System;
using TwitchSDK;
using TwitchSDK.Interop;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class EditorTwitchService : TwitchService
    {
        private EditorTwitchTestView _testView;
        
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

        public override long LastViewerCount()
        {
            throw new NotImplementedException();
        }

        public override void SyncAuthStatus()
        {
            Log("Sync status");
        }

        public override IObservable<StreamInfo> GetAndSyncStreamInfo()
        {
            throw new NotImplementedException();
        }

        private void Log(string message)
        {
            Debug.Log($"Editor Twitch Service: {message}");
        }
    }
}