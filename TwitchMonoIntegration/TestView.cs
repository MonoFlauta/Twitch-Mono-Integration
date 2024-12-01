using UnityEngine;

namespace TwitchMonoIntegration
{
    internal abstract class TestView : MonoBehaviour
    {
        protected EditorTwitchService EditorTwitchService { get; private set; }

        public void Init(EditorTwitchService editorTwitchService)
        {
            EditorTwitchService = editorTwitchService;
            Init();
        }

        protected virtual void Init(){}
    }
}