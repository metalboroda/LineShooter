namespace Assets.Scripts.Player.States
{
	public class PlayerIdleState : PlayerBaseState
	{
		public override void Enter()
		{
			PlayerMovement.Stop();
		}

		public override void Update()
		{
			if (PlayerMovement.HasMovementInput)
				PlayerController.Fsm.ChangeState(PlayerController.StateFactory.GetState<PlayerMovementState>());
		}
	}
}