using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class EditorTwitchTestView : MonoBehaviour
    {
        [Header("Control References")]
        public GameObject container;
        public GameObject buttonContainer;
        public Button buttonPrefab;
        public TestView[] testViews;
        private KeyCode _openTestViewKey;

        public void Init(EditorTwitchService editorTwitchService, KeyCode openTestViewKey)
        {
            _openTestViewKey = openTestViewKey;
            foreach (var testView in testViews)
                testView.Init(editorTwitchService);

            for (var i = testViews.Length - 1; i >= 0; i--)
            {
                var button = Instantiate(buttonPrefab, buttonContainer.transform);
                var view = testViews[i];
                button.GetComponentInChildren<TextMeshProUGUI>().text = view.gameObject.name;
                button.OnPointerClickAsObservable().Subscribe(_ =>
                {
                    TurnOffAllViews();
                    view.gameObject.SetActive(true);
                }).AddTo(button);
            }
        }

        private void TurnOffAllViews()
        {
            foreach (var testView in testViews)
                testView.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(_openTestViewKey))
                container.SetActive(!container.activeSelf);
        }
    }
}