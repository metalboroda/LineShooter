namespace Assets.Scripts.Player.States
{
	public class PlayerMovementState : PlayerBaseState
	{
		public override void Update()
		{
			if (!PlayerMovement.HasMovementInput)
			{
				PlayerController.Fsm.ChangeState(PlayerController.StateFactory.GetState<PlayerIdleState>());
				return;
			}

			PlayerMovement.Move();
		}
	}
}