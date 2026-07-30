using System;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameFlow.GameStates;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.UI.Canvases;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameManagement
{
	public class CanvasManager : MonoBehaviour
	{
		[Inject] private ISaveService _saveService;

		[Header("Canvases")]
		[SerializeField] private CanvasHandlerBase mainMenuCanvas;
		[SerializeField] private CanvasHandlerBase infoCanvas;
		[SerializeField] private CanvasHandlerBase settingsCanvas;
		[SerializeField] private CanvasHandlerBase shopCanvas;
		[SerializeField] private CanvasHandlerBase levelSelectorCanvas;
		[SerializeField] private CanvasHandlerBase gameCanvas;
		[SerializeField] private CanvasHandlerBase pauseCanvas;
		[SerializeField] private CanvasHandlerBase winCanvas;
		[SerializeField] private CanvasHandlerBase loseCanvas;
		[SerializeField] private CanvasHandlerBase tutorialCanvas;

		private readonly List<CanvasHandlerBase> _canvases = new List<CanvasHandlerBase>();
		private CanvasHandlerBase _currentCanvas;
		private CanvasHandlerBase _previousCanvas;
		private Dictionary<Type, Action> _gameStateHandlers;
		private Dictionary<UiButtonType, Action> _uiButtonHandlers;
		private CancellationTokenSource _switchCanvasCts;
		
		private EventBinding<GameEvents.GameStateChanged> _gameStateChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			AddCanvasesToList();

			BuildHandlerMaps();
		}

		private void OnEnable()
		{
			_gameStateChanged = new EventBinding<GameEvents.GameStateChanged>(OnGameStateChanged);
			EventBus<GameEvents.GameStateChanged>.Register(_gameStateChanged);
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
		}

		private void OnDisable()
		{
			EventBus<GameEvents.GameStateChanged>.Unregister(_gameStateChanged);
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
		}

		private void OnDestroy()
		{
			StopSwitchCanvasWithDelayInternal();
		}

		private void BuildHandlerMaps()
		{
			_gameStateHandlers = new Dictionary<Type, Action>
			{
				[typeof(GameMainMenuState)] = () => SwitchCanvas(mainMenuCanvas),
				[typeof(GamePreviewState)] = DisableAllCanvases,
				[typeof(GamePlayState)] = () =>
				{
					SwitchCanvas(gameCanvas);
					EnableTutorialCanvas();
				},
				[typeof(GamePauseState)] = () => SwitchCanvas(pauseCanvas),
				[typeof(GameWinState)] = () => SwitchCanvas(winCanvas),
				[typeof(GameLoseState)] = () => SwitchCanvas(loseCanvas),
			};

			_uiButtonHandlers = new Dictionary<UiButtonType, Action>
			{
				[UiButtonType.Info] = () => SwitchCanvas(infoCanvas),
				[UiButtonType.Settings] = () => SwitchCanvas(settingsCanvas),
				[UiButtonType.Shop] = () => SwitchCanvas(shopCanvas),
				[UiButtonType.LevelSelector] = () =>
				{
					DisableAllCanvases();
					SwitchCanvas(levelSelectorCanvas);
				},
				[UiButtonType.Back] = () => SwitchCanvas(_previousCanvas),
				[UiButtonType.Restart] = () =>
				{
					DisableAllCanvases();
					SwitchCanvas(gameCanvas);
				},
				[UiButtonType.NextLevel] = () =>
				{
					DisableAllCanvases();
					SwitchCanvasWithDelay(levelSelectorCanvas, 0.01f);
				},
			};
		}

		private void AddCanvasesToList()
		{
			_canvases.Add(mainMenuCanvas);
			_canvases.Add(infoCanvas);
			_canvases.Add(settingsCanvas);
			_canvases.Add(shopCanvas);
			_canvases.Add(levelSelectorCanvas);
			_canvases.Add(gameCanvas);
			_canvases.Add(pauseCanvas);
			_canvases.Add(winCanvas);
			_canvases.Add(loseCanvas);
			_canvases.Add(tutorialCanvas);

			foreach (CanvasHandlerBase canvas in _canvases)
			{
				canvas.Hide();
			}
		}

		private void OnGameStateChanged(GameEvents.GameStateChanged eventData)
		{
			if (_gameStateHandlers.TryGetValue(eventData.State.GetType(), out Action handler))
				handler();
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (_uiButtonHandlers.TryGetValue(eventData.ButtonType, out Action handler))
				handler();
		}

		private void EnableTutorialCanvas()
		{
			SettingsSave settings = _saveService.LoadSettings();

			if (settings.tutorialShown) return;

			tutorialCanvas.Show();

			settings.SaveTutorialShown(true);

			_saveService.SaveSettings(settings);
		}

		private void SwitchCanvas(CanvasHandlerBase newCanvas)
		{
			if (newCanvas is null) return;

			if (_currentCanvas == newCanvas)
			{
				if (!newCanvas.IsVisible)
				{
					newCanvas.Show();

					EventBus<UIEvents.CanvasChanged>.Raise(new UIEvents.CanvasChanged());
				}

				return;
			}

			if (_currentCanvas is not null) _previousCanvas = _currentCanvas;

			foreach (CanvasHandlerBase canvas in _canvases)
			{
				canvas.Hide();
			}

			_currentCanvas = newCanvas;

			newCanvas.Show();

			EventBus<UIEvents.CanvasChanged>.Raise(new UIEvents.CanvasChanged());
		}

		private void SwitchCanvasWithDelay(CanvasHandlerBase newCanvas, float delay)
		{
			StopSwitchCanvasWithDelayInternal();

			_switchCanvasCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
			
			DoSwitchCanvasWithDelay(newCanvas, delay, _switchCanvasCts.Token).Forget();
		}

		private async UniTaskVoid DoSwitchCanvasWithDelay(CanvasHandlerBase newCanvas, float delay, CancellationToken token)
		{
			bool wasCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token)
				.SuppressCancellationThrow();

			if (wasCancelled) return;

			SwitchCanvas(newCanvas);
		}

		private void StopSwitchCanvasWithDelayInternal()
		{
			if (_switchCanvasCts != null)
			{
				_switchCanvasCts.Cancel();
				_switchCanvasCts.Dispose();
				_switchCanvasCts = null;
			}
		}

		private void DisableAllCanvases()
		{
			foreach (CanvasHandlerBase canvas in _canvases)
			{
				canvas.Hide();
			}
		}
	}
}