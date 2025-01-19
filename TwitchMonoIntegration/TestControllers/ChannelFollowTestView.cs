using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class ChannelFollowTestView : TestView
    {
        public Button button;
        public TMP_InputField inputField;

        protected override void Init()
        {
            inputField.text = "TestUser";
            button.OnPointerClickAsObservable()
                .Subscribe(_ =>
                {
                    EditorTwitchService.ChannelFollow(inputField.text);
                }).AddTo(this);
        }
    }
}