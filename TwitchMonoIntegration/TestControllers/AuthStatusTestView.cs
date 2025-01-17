using TwitchSDK.Interop;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

namespace TwitchMonoIntegration
{
    public class AuthStatusTestView : TestView
    {
        public Button buttonLoading;
        public Button buttonWaitingForCode;
        public Button buttonLoggedIn;
        public Button buttonLoggedOut;

        protected override void Init()
        {
            SubscribeButtonWith(buttonLoading, AuthStatus.Loading);
            SubscribeButtonWith(buttonWaitingForCode, AuthStatus.WaitingForCode);
            SubscribeButtonWith(buttonLoggedIn, AuthStatus.LoggedIn);
            SubscribeButtonWith(buttonLoggedOut, AuthStatus.LoggedOut);
        }

        private void SubscribeButtonWith(Button button, AuthStatus authStatus)
        {
            button.OnPointerClickAsObservable()
                .Subscribe(_ => EditorTwitchService.AuthStatus.Value = authStatus)
                .AddTo(this);
        }
    }
}