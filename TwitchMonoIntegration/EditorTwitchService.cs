using System;
using System.Linq;
using TwitchSDK.Interop;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class EditorTwitchService : TwitchService
    {
        private readonly ISubject<CustomRewardEvent> _customRewardEvents = new Subject<CustomRewardEvent>();
        private readonly ReactiveProperty<long> _lastViewCount = new();
        private int _lastInternalViewCount;
        private readonly EditorTwitchTestView _twitchTestView;

        public EditorTwitchService(EditorTwitchTestView editorTwitchTestView, KeyCode openTestViewKey)
        {
            _twitchTestView = editorTwitchTestView;
            _twitchTestView.Init(this, openTestViewKey);
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
            GetChannelPointsRewardTestView()
                .SetRewardsWith(newRewards);
        }

        public override void ClearRewards()
        {
            GetChannelPointsRewardTestView().SetRewardsWith(Array.Empty<CustomRewardDefinition>());
        }

        private ChannelPointsRewardTestView GetChannelPointsRewardTestView() =>
            (ChannelPointsRewardTestView)_twitchTestView.testViews
                .First(x => x is ChannelPointsRewardTestView);

        public override IObservable<CustomRewardEvent> SubscribeToChannelPointRewards(bool withLogs = true) =>
            _customRewardEvents
                .Do(x => Debug.Log($"Custom Reward {x.CustomRewardTitle} claimed by {x.RedeemerName}"));

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

        public void ClaimReward(CustomRewardDefinition reward, string redeemer)
        {
            _customRewardEvents.OnNext(new CustomRewardEvent
            {
                CustomRewardTitle = reward.Title,
                CustomRewardCost = reward.Cost,
                CustomRewardPrompt = reward.Prompt,
                RedeemerName = redeemer
            });
        }
    }
}