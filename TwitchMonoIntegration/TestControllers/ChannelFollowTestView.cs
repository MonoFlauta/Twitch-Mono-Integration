using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class ChannelFollowTestView : TestView
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
                    EditorTwitchService.ChannelFollow(usernameInputField.text, idInputField.text);
                }).AddTo(this);
        }
    }
}