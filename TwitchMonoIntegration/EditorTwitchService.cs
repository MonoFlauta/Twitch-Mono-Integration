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
        private readonly ISubject<ChannelFollowEvent> _channelFollowEvents = new Subject<ChannelFollowEvent>();
        private readonly ISubject<ChannelRaidEvent> _channelRaidEvents = new Subject<ChannelRaidEvent>();
        private readonly ISubject<ChannelSubscribeEvent> _channelSubscribeEvents = new Subject<ChannelSubscribeEvent>();
        private readonly ReactiveProperty<long> _lastViewCount = new();
        private int _lastInternalViewCount;
        private readonly EditorTwitchTestView _twitchTestView;

        public EditorTwitchService(EditorTwitchTestView editorTwitchTestView, KeyCode openTestViewKey)
        {
            _twitchTestView = editorTwitchTestView;
            _twitchTestView.Init(this, openTestViewKey);
        }

        public override ISubject<string> Initialize(MonoBehaviour monoBehaviour)
        {
            Initialized.Value = true;
            AuthStatus.Value = TwitchSDK.Interop.AuthStatus.Loading;
            return new Subject<string>();
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
                .Do(x =>
                {
                    if(withLogs)
                        Debug.Log($"Custom Reward {x.CustomRewardTitle} claimed by {x.RedeemerName}");
                });

        public override IObservable<ChannelFollowEvent> SubscribeToChannelFollows(bool withLogs = true) =>
            _channelFollowEvents
                .Do(x =>
                {
                    if (withLogs)
                        Debug.Log($"Channel Follow {x.UserDisplayName}");
                });

        public override IObservable<ChannelSubscribeEvent> SubscribeToChannelSubscribe(bool withLogs = true) =>
            _channelSubscribeEvents
                .Do(x =>
                {
                    if(withLogs)
                        Debug.Log($"Channel Subscribe {x.UserDisplayName}");
                });

        public override IObservable<HypeTrainEvent> SubscribeToHypeTrain(bool withLogs = true)
        {
            throw new NotImplementedException();
        }

        public override IObservable<ChannelRaidEvent> SubscribeToChannelRaid(bool withLogs = true) =>
            _channelRaidEvents
                .Do(x =>
                {
                    if(withLogs)
                        Debug.Log($"Raid from {x.FromBroadcasterName} with {x.Viewers} viewers");
                });

        public override IObservable<ClipInfo> CreateClip(bool withDelay = false) =>
            new Subject<ClipInfo>().Do(_ =>
            {
                Debug.Log("Clip Created");
            });

        public void SetViewerCountTo(int viewerCount)
        {
            _lastInternalViewCount = viewerCount;
        }

        private void Log(string message)
        {
            Debug.Log($"Editor Twitch Service: {message}");
        }

        public void ClaimReward(CustomRewardDefinition reward, string redeemer, string id)
        {
            _customRewardEvents.OnNext(new CustomRewardEvent
            {
                CustomRewardTitle = reward.Title,
                CustomRewardCost = reward.Cost,
                CustomRewardPrompt = reward.Prompt,
                RedeemerName = redeemer,
                RedeemerId = id
            });
        }

        public void ChannelFollow(string username, string userId)
        {
            _channelFollowEvents.OnNext(new ChannelFollowEvent
            {
                UserDisplayName = username,
                UserId = userId
            });
        }

        public void ChannelRaid(string fromChannel, int viewers)
        {
            _channelRaidEvents.OnNext(new ChannelRaidEvent
            {
                FromBroadcasterName = fromChannel,
                Viewers = viewers
            });
        }

        public void ChannelSubscribe(string username, string id)
        {
            _channelSubscribeEvents.OnNext(new ChannelSubscribeEvent
            {
                UserDisplayName = username,
                UserId = id
            });
        }
    }
}