using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    internal class VisitorCountTestView : TestView
    {
        public TMP_InputField countInputField;
        public Button updateButton;

        protected override void Init()
        {
            base.Init();

            updateButton.OnPointerClickAsObservable()
                .Subscribe(_ => EditorTwitchService.SetViewerCountTo(int.Parse(countInputField.text)))
                .AddTo(this);
        }
    }
}