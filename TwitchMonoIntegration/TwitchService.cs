using System;
using TwitchSDK.Interop;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public abstract class TwitchService
    {
        /// <summary>
        /// Tells you if the service has been initialized. You shouldn't do anything without this
        /// </summary>
        public readonly ReactiveProperty<bool> Initialized = new(false);
        /// <summary>
        /// Tells you the status of the authentication. Until it is LoggedIn you will not be able to use any of the features
        /// </summary>
        public readonly ReactiveProperty<AuthStatus> AuthStatus = new(TwitchSDK.Interop.AuthStatus.Loading);
        /// <summary>
        /// Called by the Twitch Controller. Unless you are using your own implementation, you won't need this one. It returns the code for authentication if needed.
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <returns>Subject with the code for authentication if needed</returns>
        public abstract ISubject<string> Initialize(MonoBehaviour monoBehaviour);
        /// <summary>
        /// Lets you start a new simple pool
        /// </summary>
        /// <param name="title">Title for the pool</param>
        /// <param name="choices">Choices for the pool</param>
        /// <param name="duration">Duration, in seconds, for the pool</param>
        /// <returns>Returns a TwitchPool that contains the results and status</returns>
        public abstract TwitchPool NewSimplePool(string title, string[] choices, long duration);
        /// <summary>
        /// Lets you know whenever the viewer count changes
        /// </summary>
        /// <returns>Returns the viewer count of the stream in an observable</returns>
        public abstract IObservable<long>  ObserveViewerCount();
        /// <summary>
        /// Returns the last viewer count
        /// </summary>
        /// <returns>Viewer count</returns>
        public abstract long LastViewerCount();
        /// <summary>
        /// Syncs auth status
        /// </summary>
        public abstract void SyncAuthStatus();
        /// <summary>
        /// Syncs and gets stream info
        /// </summary>
        /// <returns>Stream info</returns>
        public abstract IObservable<StreamInfo> GetAndSyncStreamInfo();
        /// <summary>
        /// Sets custom rewards
        /// </summary>
        /// <param name="newRewards">New rewards</param>
        public abstract void SetCustomRewards(CustomRewardDefinition[] newRewards);
        /// <summary>
        /// Clears current rewards
        /// </summary>
        public abstract void ClearRewards();
        /// <summary>
        /// Subscribes to Channel Point Rewards. First time initializes feature
        /// </summary>
        /// <param name="withLogs">If it should include logs (default = true)</param>
        /// <returns>Observable for events</returns>
        public abstract IObservable<CustomRewardEvent> SubscribeToChannelPointRewards(bool withLogs = true);
        /// <summary>
        /// Subscribes to Channel Follows. First time initializes feature
        /// </summary>
        /// <param name="withLogs">If it should include logs (default = true)</param>
        /// <returns>Observable for events</returns>
        public abstract IObservable<ChannelFollowEvent> SubscribeToChannelFollows(bool withLogs = true);
        /// <summary>
        /// Subscribes to Channel Subscribes. First time initializes feature
        /// </summary>
        /// <param name="withLogs">If it should include logs (default = true)</param>
        /// <returns>Observable for events</returns>
        public abstract IObservable<ChannelSubscribeEvent> SubscribeToChannelSubscribe(bool withLogs = true);
        /// <summary>
        /// Subscribes to Channel Hype Train. First time initializes feature
        /// </summary>
        /// <param name="withLogs">If it should include logs (default = true)</param>
        /// <returns>Observable for events</returns>
        public abstract IObservable<HypeTrainEvent> SubscribeToHypeTrain(bool withLogs = true);
        /// <summary>
        /// Subscribes to Channel Raids. First time initializes feature
        /// </summary>
        /// <param name="withLogs">If it should include logs (default = true)</param>
        /// <returns>Observable for events</returns>
        public abstract IObservable<ChannelRaidEvent> SubscribeToChannelRaid(bool withLogs = true);
        /// <summary>
        /// Creates a clip from "about 85 seconds of the stream before the call and about 5 seconds after the call"
        /// </summary>
        /// <param name="withDelay">"If false, the API captures the clip at the point in time that the viewer requests it (this is the same experience that the Twitch UX provides). If true, Twitch adds a delay before capturing the clip, which basically shifts the capture window to the right slightly"</param>
        /// <returns>Returns the observable to create the clip</returns>
        public abstract IObservable<ClipInfo> CreateClip(bool withDelay = false);
    }
}