using TwitchSDK.Interop;

namespace TwitchMonoIntegration
{
    public class RealTwitchEventSubscribeSubscriber : RealTwitchEventSubscriber<ChannelSubscribeEvent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            task = Twitch.API.SubscribeToChannelSubscribeEvents();
        }

        protected override string LogMessage(ChannelSubscribeEvent e) => 
            $"{e.UserDisplayName} has subscribed!";
    }
}