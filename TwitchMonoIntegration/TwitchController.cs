using System;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class TwitchController : MonoBehaviour
    {
        //GUIDE: https://dev.twitch.tv/docs/game-engine-plugins/unity-guide/
        //TODO: Check rate limits https://dev.twitch.tv/docs/api/guide/#twitch-rate-limits

        public static TwitchController Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = Instantiate(Resources.Load<TwitchController>("TwitchController"));
                return _instance;
            }
        }
        private static TwitchController _instance;
        public TwitchService Service;

        public IObservable<Unit> Initialize(bool useEditor = false)
        {
            if (useEditor)
                Service = new EditorTwitchService();
            else
                Service = new RealTwitchService();
            return Service.Initialized.DoOnSubscribe(() =>
            {
                Service.Initialize(this);
            }).First(x => x).AsUnitObservable();
        }
    }
}
