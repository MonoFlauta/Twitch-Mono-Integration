using TMPro;
using TwitchSDK.Interop;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class ChannelPointsRewardTestView : TestView
    {
        public Button buttonPrefab;
        public TMP_InputField usernameInputField;
        public TMP_InputField idInputField;
        public GameObject buttonContainer;

        protected override void Init()
        {
            usernameInputField.text = "TestUser";
        }

        public void SetRewardsWith(CustomRewardDefinition[] rewards)
        {
            foreach (var oldButton in buttonContainer.GetComponentsInChildren<Button>())
                Destroy(oldButton);
            foreach (var reward in rewards)
            {
                var button = Instantiate(buttonPrefab, buttonContainer.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = reward.Title;
                button.OnPointerClickAsObservable().Subscribe(_ =>
                {
                    EditorTwitchService.ClaimReward(reward, usernameInputField.text, idInputField.text);
                }).AddTo(button);
            }
        }
    }
}