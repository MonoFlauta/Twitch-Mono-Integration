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

        public IObservable<string> Initialize(bool useEditor = false, KeyCode openTestViewKey = KeyCode.Alpha1)
        {
            if (useEditor)
                service = new EditorTwitchService(editorTwitchTestView, openTestViewKey);
            else
                service = new RealTwitchService();
            return service.Initialize(this);
        }

        public IObservable<Unit> OnInitialized() => 
            service.Initialized
                .First(x => x)
                .AsUnitObservable();
    }
}
