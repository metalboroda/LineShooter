using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;

namespace Assets.Scripts.EventsFolder
{
	public static class UIEvents
	{
		public struct UIButtonClicked : IEvent
		{
			public UiButtonType ButtonType;
		}

		public struct CanvasChanged : IEvent {}

		public struct SelectorItemPlayPressed : IEvent
		{
			public int Index;
			public int Rating;
		}

		public struct UiMusicClicked : IEvent {}

		public struct UiSfxClicked : IEvent {}

		public struct VibrationClicked : IEvent {}

		public struct TutorialCompleted : IEvent {}
	}
}