using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Input
{
	public class InputReader : MonoBehaviour, IMovementInput
	{
		[SerializeField] private InputActionReference movementAction;

		public float Horizontal { get; private set; }

		private void OnEnable() => movementAction.action.Enable();
		private void OnDisable() => movementAction.action.Disable();

		private void Update()
		{
			Horizontal = movementAction.action.ReadValue<float>();
		}
	}
}