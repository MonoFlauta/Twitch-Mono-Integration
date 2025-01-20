using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class ChannelSubscribeTestView : TestView
    {
        public Button button;
        public TMP_InputField usernameInputField;
        public TMP_InputField idInputField;

        protected override void Init()
        {
            usernameInputField.text = "TestUser";
            button.OnPointerClickAsObservable()
                .Subscribe(_ =>
                {
                    EditorTwitchService.ChannelSubscribe(usernameInputField.text, idInputField.text);
                }).AddTo(this);
        }
    }
}