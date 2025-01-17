using System;
using TwitchSDK.Interop;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class EditorTwitchService : TwitchService
    {
        private ReactiveProperty<long> _lastViewCount;
        private int _lastInternalViewCount;

        public EditorTwitchService(EditorTwitchTestView editorTwitchTestView)
        {
            editorTwitchTestView.Init(this);
        }

        public override void Initialize(MonoBehaviour monoBehaviour)
        {
            Initialized.Value = true;
            AuthStatus.Value = TwitchSDK.Interop.AuthStatus.Loading;
        }

        public override TwitchPool NewSimplePool(string title, string[] choices, long duration)
        {
            throw new NotImplementedException();
        }

        public override IObservable<long> ObserveViewerCount() =>
            _lastViewCount
                .AsObservable();

        public override long LastViewerCount() => 
            _lastViewCount.Value;

        public override void SyncAuthStatus()
        {
            Log("Sync status");
        }

        public override IObservable<StreamInfo> GetAndSyncStreamInfo() =>
            Observable.Return(new StreamInfo
                {
                    ViewerCount =  _lastInternalViewCount
                })
                .Do(x => _lastViewCount.Value = x.ViewerCount);

        public override void SetCustomRewards(CustomRewardDefinition[] newRewards)
        {
            throw new NotImplementedException();
        }

        public override void ClearRewards()
        {
            throw new NotImplementedException();
        }

        public override IObservable<CustomRewardEvent> SubscribeToChannelPointRewards(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

        public override IObservable<ChannelFollowEvent> SubscribeToChannelFollows(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

        public override IObservable<ChannelSubscribeEvent> SubscribeToChannelSubscribe(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

        public override IObservable<HypeTrainEvent> SubscribeToHypeTrain(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

        public override IObservable<ChannelRaidEvent> SubscribeToChannelRaid(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

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