using TwitchSDK.Interop;

namespace TwitchMonoIntegration
{
    public class RealTwitchHypeTrainSubscriber : RealTwitchEventSubscriber<HypeTrainEvent>
    {
        protected override void Initialize()
        {
            base.Initialize();
            task = Twitch.API.SubscribeToHypeTrainEvents();
        }

        protected override string LogMessage(HypeTrainEvent e) => 
            $"The Hype train level is {e.Level}";
    }
}