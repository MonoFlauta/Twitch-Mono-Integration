using System;
using TwitchSDK;
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
        /// Called by the Twitch Controller. Unless you are using your own implementation, you won't need this one
        /// </summary>
        /// <param name="monoBehaviour"></param>
        public abstract void Initialize(MonoBehaviour monoBehaviour);
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

        public abstract void SyncAuthStatus();
        public abstract IObservable<StreamInfo> GetAndSyncStreamInfo();
    }
}