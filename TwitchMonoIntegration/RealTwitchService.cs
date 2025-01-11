using System;
using JetBrains.Annotations;
using TwitchSDK;
using TwitchSDK.Interop;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    [UsedImplicitly]
    public class RealTwitchService : TwitchService
    {
        private GameTask<AuthenticationInfo> _authenticationInfo;
        private long _lastViewerCount = 0;
        private RealTwitchEventCustomRewardSubscriber _customRewardsSubscriber;
        private RealTwitchEventFollowsSubscriber _eventFollowsSubscriber;
        private RealTwitchEventSubscribeSubscriber _subscribeSubscriber;
        private RealTwitchHypeTrainSubscriber _hypeTrainSubscriber;
        private RealTwitchRaidSubscriber _raidSubscriber;

        public override void Initialize(MonoBehaviour monoBehaviour)
        {
            _authenticationInfo = GetAuthenticationInfo();

            AuthStatus
                .First(x => x == TwitchSDK.Interop.AuthStatus.WaitingForCode)
                .Subscribe(_ =>
                {
                    _authenticationInfo.ObserveEveryValueChanged(x => x.MaybeResult)
                        .Where(x => x != null)
                        .First()
                        .Subscribe(x =>
                        {
                            Application.OpenURL($"{_authenticationInfo.MaybeResult.Uri}{_authenticationInfo.MaybeResult.UserCode}");
                            Log($"User code: {_authenticationInfo.MaybeResult.UserCode}");
                        });
                })
                .AddTo(monoBehaviour);
            
            var disposable = Observable
                .Interval(TimeSpan.FromSeconds(2))
                .Subscribe(_ => TwitchController.Instance.Service.SyncAuthStatus())
                .AddTo(monoBehaviour);
            AuthStatus
                .Subscribe(x =>
                {
                    Log($"Auth status updated to {x}");
                    if (x != TwitchSDK.Interop.AuthStatus.LoggedIn) return;
                    Initialized.Value = true;
                    disposable?.Dispose();
                }).AddTo(monoBehaviour);
        }

        public override TwitchPool NewSimplePool(string title, string[] choices, long duration) =>
            new RealTwitchPool(Twitch.API.NewPoll(new PollDefinition
            {
                Title = title,
                Choices = choices,
                Duration = duration
            }));

        public override IObservable<long> ObserveViewerCount() => 
            this.ObserveEveryValueChanged(x => x._lastViewerCount);

        public override long LastViewerCount() => 
            _lastViewerCount;

        public override void SyncAuthStatus()
        {
            Twitch.API.GetAuthState().Task.ToObservable().Do(x => AuthStatus.Value = x.Status).Subscribe();
        }

        public override IObservable<StreamInfo> GetAndSyncStreamInfo() => 
            Twitch.API.GetMyStreamInfo().Task
                .ToObservable()
                .Do(x => _lastViewerCount = x.ViewerCount);

        public override void SetCustomRewards(CustomRewardDefinition[] newRewards)
        {
            Twitch.API.ReplaceCustomRewards(newRewards);
        }

        public override void ClearRewards()
        {
            Twitch.API.ReplaceCustomRewards(new[] { new CustomRewardDefinition() });
        }

        public override IObservable<CustomRewardEvent> SubscribeToChannelPointRewards(bool withLogs = true)
        {
            _customRewardsSubscriber ??= RealTwitchEventSubscriber<CustomRewardEvent>.CreateInstance<RealTwitchEventCustomRewardSubscriber>(withLogs);
            return _customRewardsSubscriber.OnReceiveEvent();
        }

        public override IObservable<ChannelFollowEvent> SubscribeToChannelFollows(bool withLogs = true)
        {
            _eventFollowsSubscriber ??= RealTwitchEventSubscriber<ChannelFollowEvent>.CreateInstance<RealTwitchEventFollowsSubscriber>(withLogs);
            return _eventFollowsSubscriber.OnReceiveEvent();
        }

        public override IObservable<ChannelSubscribeEvent> SubscribeToChannelSubscribe(bool withLogs = true)
        {
            _subscribeSubscriber ??= RealTwitchEventSubscriber<ChannelSubscribeEvent>.CreateInstance<RealTwitchEventSubscribeSubscriber>(withLogs);
            return _subscribeSubscriber.OnReceiveEvent();
        }

        public override IObservable<HypeTrainEvent> SubscribeToHypeTrain(bool withLogs = true)
        {
            _hypeTrainSubscriber ??= RealTwitchEventSubscriber<HypeTrainEvent>.CreateInstance<RealTwitchHypeTrainSubscriber>(withLogs);
            return _hypeTrainSubscriber.OnReceiveEvent();
        }

        public override IObservable<ChannelRaidEvent> SubscribeToChannelRaid(bool withLogs = true)
        {
             _raidSubscriber ??= RealTwitchEventSubscriber<ChannelRaidEvent>.CreateInstance<RealTwitchRaidSubscriber>(withLogs);
            return _raidSubscriber.OnReceiveEvent();
        }

        private GameTask<AuthenticationInfo> GetAuthenticationInfo() => 
            Twitch.API.GetAuthenticationInfo(new TwitchOAuthScope(GetScopes()));

        private static string GetScopes() => 
            TwitchOAuthScope.Bits.Read.Scope + " " + TwitchOAuthScope.Channel.ManageBroadcast.Scope + " " + TwitchOAuthScope.Channel.ManagePolls.Scope + " " + TwitchOAuthScope.Channel.ManagePredictions.Scope + " " + TwitchOAuthScope.Channel.ManageRedemptions.Scope + " " + TwitchOAuthScope.Channel.ReadHypeTrain.Scope + " " + TwitchOAuthScope.Clips.Edit.Scope + " " + TwitchOAuthScope.User.ReadSubscriptions.Scope;

        private void Log(string message)
        {
            Debug.Log($"Real Twitch Service: {message}");
        }
    }
}