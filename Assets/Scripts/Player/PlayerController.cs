using Assets.Scripts.FSM;
using Assets.Scripts.Player.States;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Inject] private DiContainer _diContainer;

		[SerializeField] private PlayerMovement playerMovement;

		public PlayerMovement Movement => playerMovement;

		public IFiniteStateMachine<PlayerController> Fsm { get; private set; }
		public StateFactory<PlayerController> StateFactory { get; private set; }

		private void Awake()
		{
			Fsm = new FiniteStateMachine<PlayerController>(this);
			StateFactory = new StateFactory<PlayerController>(this, _diContainer);
		}

		private void Start()
		{
			Fsm.Initialize(StateFactory.GetState<PlayerIdleState>());
		}

		private void Update()
		{
			Fsm.CurrentState?.Update();
		}

		private void FixedUpdate()
		{
			Fsm.CurrentState?.FixedUpdate();
		}
	}
}