using TwitchSDK.Interop;

namespace TwitchMonoIntegration
{
    public class RealTwitchEventCustomRewardSubscriber : RealTwitchEventSubscriber<CustomRewardEvent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            task = Twitch.API.SubscribeToCustomRewardEvents();
        }

        protected override string LogMessage(CustomRewardEvent e) => 
            $"{e.RedeemerName} has brought {e.CustomRewardTitle} for {e.CustomRewardCost}";
    }
}