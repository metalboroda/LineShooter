using Assets.Scripts.EventBus;
using Assets.Scripts.FSM;
using Assets.Scripts.GameManagement;

namespace Assets.Scripts.EventsFolder
{
	public static class GameEvents
	{
		public struct GameStateChanged : IEvent
		{
			public IState<GameStateManager> State;
		}
	}
}