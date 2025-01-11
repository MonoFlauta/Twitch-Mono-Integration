using UnityEngine;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    internal class EditorTwitchTestView : MonoBehaviour
    {
        [Header("Control References")]
        public KeyCode keyCode = KeyCode.Alpha1;
        public GameObject container;
        public Button buttonPrefab;
        public TestView[] testViews;
        
        public void Init(EditorTwitchService editorTwitchService)
        {
            foreach (var testView in testViews)
                testView.Init(editorTwitchService);
        }
    }
}