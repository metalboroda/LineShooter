using System;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.FSM;
using Assets.Scripts.GameManagement.GameStates;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
	public class GameStateManager : MonoBehaviour
	{
		public IFiniteStateMachine<GameStateManager> Fsm { get; private set; }
		public StateFactory<GameStateManager> StateFactory { get; private set; }

		private Dictionary<UiButtonType, Func<IState<GameStateManager>>> _uiButtonStateMap;
		
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;
		private EventBinding<UIEvents.SelectorItemPlayPressed> _selectorItemPlayPressed;

		private void Awake()
		{
			Fsm = new FiniteStateMachine<GameStateManager>(this);
			StateFactory = new StateFactory<GameStateManager>(this);

			Fsm.StateChanged += OnStateChanged;

			_uiButtonStateMap = new Dictionary<UiButtonType, Func<IState<GameStateManager>>>
			{
				[UiButtonType.MainMenu] = StateFactory.GetState<GameMainMenuState>,
				[UiButtonType.Pause] = StateFactory.GetState<GamePauseState>,
				[UiButtonType.Restart] = StateFactory.GetState<GamePlayState>,
				[UiButtonType.ResumeGame] = StateFactory.GetState<GamePlayState>,
				[UiButtonType.NextLevel] = StateFactory.GetState<GameMainMenuState>,
			};
		}

		private void OnEnable()
		{
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
			_selectorItemPlayPressed = new EventBinding<UIEvents.SelectorItemPlayPressed>(OnSelectorItemPlayPressed);
			EventBus<UIEvents.SelectorItemPlayPressed>.Register(_selectorItemPlayPressed);
		}

		private void Start()
		{
			Fsm.Initialize(StateFactory.GetState<GameMainMenuState>());
		}

		private void OnDisable()
		{
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
			EventBus<UIEvents.SelectorItemPlayPressed>.Unregister(_selectorItemPlayPressed);
		}

		private void OnDestroy()
		{
			Fsm.StateChanged -= OnStateChanged;
		}

		private void OnStateChanged(IState<GameStateManager> newState)
		{
			EventBus<GameEvents.GameStateChanged>.Raise(new GameEvents.GameStateChanged
			{
				State = newState
			});
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (_uiButtonStateMap.TryGetValue(eventData.ButtonType, out Func<IState<GameStateManager>> getState))
			{
				Fsm.ChangeState(getState());
			}
		}

		private void OnSelectorItemPlayPressed()
		{
			Fsm.ChangeState(StateFactory.GetState<GamePlayState>());
		}
	}
}
