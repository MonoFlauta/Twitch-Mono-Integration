using TwitchSDK.Interop;

namespace TwitchMonoIntegration
{
    public class RealTwitchEventFollowsSubscriber : RealTwitchEventSubscriber<ChannelFollowEvent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            task = Twitch.API.SubscribeToChannelFollowEvents();
        }

        protected override string LogMessage(ChannelFollowEvent e) => 
            $"{e.UserDisplayName} is now following!";
    }
}