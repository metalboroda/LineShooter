using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameManagement.GameStates;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.UI.Canvases;
using UnityEngine;

namespace Assets.Scripts.GameManagement
{
	public class CanvasManager : MonoBehaviour
	{
		[Header("Canvases")]
		[SerializeField] private GameObject mainMenuCanvas;
		[SerializeField] private GameObject infoCanvas;
		[SerializeField] private GameObject settingsCanvas;
		[SerializeField] private GameObject shopCanvas;
		[SerializeField] private GameObject levelSelectorCanvas;
		[SerializeField] private GameObject gameCanvas;
		[SerializeField] private GameObject pauseCanvas;
		[SerializeField] private GameObject winCanvas;
		[SerializeField] private GameObject loseCanvas;
		[SerializeField] private GameObject tutorialCanvas;

		private readonly List<GameObject> _canvases = new();
		private GameObject _currentCanvas;
		private GameObject _previousCanvas;

		private EventBinding<GameEvents.GameStateChanged> _gameStateChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			AddCanvasesToList();

			tutorialCanvas.GetComponent<TutorialCanvasHandler>();
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

			foreach (GameObject canvas in _canvases)
			{
				canvas.SetActive(false);
			}
		}

		private void OnGameStateChanged(GameEvents.GameStateChanged eventData)
		{
			switch (eventData.State)
			{
				case GameMainMenuState:
					SwitchCanvas(mainMenuCanvas);
					break;
				case GamePreviewState:
					DisableAllCanvases();
					break;
				case GamePlayState:
					SwitchCanvas(gameCanvas);
					EnableTutorialCanvas();
					break;
				case GamePauseState:
					SwitchCanvas(pauseCanvas);
					break;
				case GameWinState:
					SwitchCanvas(winCanvas);
					break;
				case GameLoseState:
					SwitchCanvas(loseCanvas);
					break;
			}
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			switch (eventData.ButtonType)
			{
				case UiButtonType.Default:
					break;
				case UiButtonType.Info:
					SwitchCanvas(infoCanvas);
					break;
				case UiButtonType.Settings:
					SwitchCanvas(settingsCanvas);
					break;
				case UiButtonType.Shop:
					SwitchCanvas(shopCanvas);
					break;
				case UiButtonType.LevelSelector:
					DisableAllCanvases();
					SwitchCanvas(levelSelectorCanvas);
					break;
				case UiButtonType.Back:
					SwitchCanvas(_previousCanvas);
					break;
				case UiButtonType.Restart:
					DisableAllCanvases();
					SwitchCanvas(gameCanvas);
					break;
				case UiButtonType.NextLevel:
					DisableAllCanvases();
					SwitchCanvasWithDelay(levelSelectorCanvas, 0.01f);
					break;
			}
		}

		private void EnableTutorialCanvas()
		{
			SettingsSave settings = SaveManager.LoadSettings();

			if (settings.tutorialShown) return;

			tutorialCanvas.SetActive(true);

			settings.SaveTutorialShown(true);

			SaveManager.SaveSettings(settings);
		}

		private void SwitchCanvas(GameObject newCanvas)
		{
			if (_currentCanvas == newCanvas)
			{
				if (!newCanvas.activeSelf)
				{
					newCanvas.SetActive(true);

					EventBus<UIEvents.CanvasChanged>.Raise(new UIEvents.CanvasChanged());
				}

				return;
			}

			if (_currentCanvas is not null) _previousCanvas = _currentCanvas;

			foreach (GameObject canvas in _canvases)
			{
				canvas.SetActive(false);
			}

			_currentCanvas = newCanvas;

			newCanvas.SetActive(true);

			EventBus<UIEvents.CanvasChanged>.Raise(new UIEvents.CanvasChanged());
		}

		public void SwitchCanvasWithDelay(GameObject newCanvas, float delay)
		{
			StartCoroutine(DoSwitchCanvasWithDelay(newCanvas, delay));
		}

		private IEnumerator DoSwitchCanvasWithDelay(GameObject newCanvas, float delay)
		{
			yield return new WaitForSeconds(delay);
			SwitchCanvas(newCanvas);
		}

		private void DisableAllCanvases()
		{
			foreach (GameObject canvas in _canvases)
			{
				canvas.SetActive(false);
			}
		}
	}
}