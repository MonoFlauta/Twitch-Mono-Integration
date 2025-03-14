﻿using System;
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
        private long _lastViewerCount;
        private RealTwitchEventCustomRewardSubscriber _customRewardsSubscriber;
        private RealTwitchEventFollowsSubscriber _eventFollowsSubscriber;
        private RealTwitchEventSubscribeSubscriber _subscribeSubscriber;
        private RealTwitchHypeTrainSubscriber _hypeTrainSubscriber;
        private RealTwitchRaidSubscriber _raidSubscriber;

        public override ISubject<string> Initialize(MonoBehaviour monoBehaviour)
        {
            _authenticationInfo = GetAuthenticationInfo();
            ISubject<string> result = new Subject<string>();

            AuthStatus
                .Where(x => x == TwitchSDK.Interop.AuthStatus.LoggedOut)
                .Subscribe(_ =>
                {
                    GetAuthenticationInfo();
                }).AddTo(monoBehaviour);

            AuthStatus
                .Where(x => x == TwitchSDK.Interop.AuthStatus.WaitingForCode)
                .Subscribe(_ =>
                {
                    if (_authenticationInfo.MaybeResult != null)
                        OnReceiveCode(result);
                    else
                        _authenticationInfo.ObserveEveryValueChanged(x => x.MaybeResult)
                            .Where(x => x != null)
                            .Subscribe(_ => { OnReceiveCode(result); });
                })
                .AddTo(monoBehaviour);
            
            var disposable = Observable
                .Interval(TimeSpan.FromSeconds(2))
                .Subscribe(_ => TwitchController.Instance.service.SyncAuthStatus())
                .AddTo(monoBehaviour);
            AuthStatus
                .Subscribe(x =>
                {
                    Log($"Auth status updated to {x}");
                    if (x != TwitchSDK.Interop.AuthStatus.LoggedIn) return;
                    Initialized.Value = true;
                    disposable?.Dispose();
                }).AddTo(monoBehaviour);

            return result;
        }

        private void OnReceiveCode(ISubject<string> result)
        {
            result.OnNext(_authenticationInfo.MaybeResult.UserCode);
            Application.OpenURL($"{_authenticationInfo.MaybeResult.Uri}");
        }

        public override TwitchPoll NewSimplePoll(string title, string[] choices, long duration) =>
            new RealTwitchPoll(Twitch.API.NewPoll(new PollDefinition
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
            Twitch.API.ReplaceCustomRewards(new CustomRewardDefinition());
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

        public override IObservable<ClipInfo> CreateClip(bool withDelay = false) => 
            Twitch.API.CreateClip(withDelay).Task.ToObservable();

        private GameTask<AuthenticationInfo> GetAuthenticationInfo() => 
            Twitch.API.GetAuthenticationInfo(Resources.Load<MonoTwitchSettingsScriptableObject>("TwitchMonoSettings").GetAuthScope);

        private void Log(string message)
        {
            Debug.Log($"Real Twitch Service: {message}");
        }
    }
}