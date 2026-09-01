using Assets.Scripts.FSM;

namespace Assets.Scripts.Player.States
{
	public class PlayerBaseState : State<PlayerController>
	{
		protected PlayerController PlayerController => Context;
		protected PlayerMovement PlayerMovement => Context.Movement;
	}
}