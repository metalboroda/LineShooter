using Assets.Scripts.EventBus;

namespace Assets.Scripts.EventsFolder
{
	public static class AudioEvents
	{
		public struct VoiceoverPlayed : IEvent
		{
			public bool IsVoiceoverPlayed;
		}
	}
}