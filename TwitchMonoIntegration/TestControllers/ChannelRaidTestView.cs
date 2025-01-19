using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class ChannelRaidTestView : TestView
    {
        public Button button;
        public TMP_InputField channelNameInputField;
        public TMP_InputField viewersCountInputField;
        
        protected override void Init()
        {
            channelNameInputField.text = "TestChannel";
            viewersCountInputField.text = "0";
            button.OnPointerClickAsObservable().Subscribe(
                _ =>
                {
                    EditorTwitchService.ChannelRaid(channelNameInputField.text, int.Parse(viewersCountInputField.text));
                }).AddTo(this);
        }
    }
}