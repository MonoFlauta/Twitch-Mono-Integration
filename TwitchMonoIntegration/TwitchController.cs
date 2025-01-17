using System;
using UniRx;
using UnityEngine;

namespace TwitchMonoIntegration
{
    public class TwitchController : MonoBehaviour
    {
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
        public TwitchService service;
        public EditorTwitchTestView editorTwitchTestView;

        public IObservable<Unit> Initialize(bool useEditor = false)
        {
            if (useEditor)
                service = new EditorTwitchService(editorTwitchTestView);
            else
                service = new RealTwitchService();
            return service.Initialized.DoOnSubscribe(() =>
            {
                service.Initialize(this);
            }).First(x => x).AsUnitObservable();
        }
    }
}
