using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameFlow.GameStates;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.GameManagement
{
	public class LevelManager : MonoBehaviour
	{
		[Inject] private ISaveService _saveService;
		[Inject] private LevelDataSo _levelDataSo;

		[Header("References")]
		[SerializeField] private GameObject[] levels;

		private GameObject _spawnedLevel;
		private int _currentLevelIndex;
		private int _currentLevelRating;

		private EventBinding<UIEvents.SelectorItemPlayPressed> _selectorItemPlayPressed;
		private EventBinding<GameEvents.GameStateChanged> _gameStateChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void OnEnable()
		{
			_selectorItemPlayPressed = new EventBinding<UIEvents.SelectorItemPlayPressed>(OnSelectorItemPlayPressed);
			EventBus<UIEvents.SelectorItemPlayPressed>.Register(_selectorItemPlayPressed);
			_gameStateChanged = new EventBinding<GameEvents.GameStateChanged>(OnGameStateChanged);
			EventBus<GameEvents.GameStateChanged>.Register(_gameStateChanged);
			_uiButtonClicked = new EventBinding<UIEvents.UIButtonClicked>(OnUiButtonClicked);
			EventBus<UIEvents.UIButtonClicked>.Register(_uiButtonClicked);
		}

		private void OnDisable()
		{
			EventBus<UIEvents.SelectorItemPlayPressed>.Unregister(_selectorItemPlayPressed);
			EventBus<GameEvents.GameStateChanged>.Unregister(_gameStateChanged);
			EventBus<UIEvents.UIButtonClicked>.Unregister(_uiButtonClicked);
		}

		private void OnSelectorItemPlayPressed(UIEvents.SelectorItemPlayPressed eventData)
		{
			_currentLevelIndex = eventData.Index;
			_currentLevelRating = eventData.Rating;

			SpawnLevel(_currentLevelIndex);
		}

		private void OnGameStateChanged(GameEvents.GameStateChanged eventData)
		{
			if (eventData.State is not GameMainMenuState) return;
			if (_spawnedLevel is not null)
				Destroy(_spawnedLevel);

			_currentLevelIndex = 0;
			_currentLevelRating = 0;
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (eventData.ButtonType != UiButtonType.Restart) return;

			Destroy(_spawnedLevel);
			SpawnLevel(_currentLevelIndex);
		}

		private void SpawnLevel(int index)
		{
			if (index < 0 || index >= levels.Length) return;

			_spawnedLevel = Instantiate(levels[index]);

			_currentLevelIndex = index;
		}

		private void CalculateLevelRating()
		{
			// Hardcode
			_currentLevelRating = 3;

			SaveLevelRating();
		}

		private void SaveLevelRating()
		{
			_levelDataSo.CurrentLevelRating = _currentLevelRating;

			LevelSave levelSave = _saveService.LoadLevelSettings();

			levelSave.SetLevelRating(_currentLevelIndex, _currentLevelRating);

			_saveService.SaveLevelSettings(levelSave);
		}
	}
}