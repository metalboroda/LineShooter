using Assets.Scripts.FSM;
using Assets.Scripts.GameManagement;

namespace Assets.Scripts.GameFlow.GameStates
{
	public class GameBaseState : State<GameStateManager>
	{
		protected GameStateManager GameStateManager => Context;
	}
}