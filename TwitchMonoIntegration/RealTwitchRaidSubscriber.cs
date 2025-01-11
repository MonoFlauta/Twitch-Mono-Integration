using TwitchSDK.Interop;

namespace TwitchMonoIntegration
{
    public class RealTwitchRaidSubscriber : RealTwitchEventSubscriber<ChannelRaidEvent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            task = Twitch.API.SubscribeToChannelRaidEvents();
        }

        protected override string LogMessage(ChannelRaidEvent e) => 
            $"{e.FromBroadcasterName} has raided with {e.Viewers} people!";
    }
}