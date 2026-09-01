using System.Threading;
using Assets.Scripts.Enums;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Assets.Scripts.GameFlow.GameStates;
using Assets.Scripts.GameManagement.LevelLoading;
using Assets.Scripts.SaveSystem;
using Assets.Scripts.ScriptableObjects.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Scripts.GameManagement
{
	public class LevelManager : MonoBehaviour
	{
		[Inject] private ISaveService _saveService;
		[Inject] private LevelDataSo _levelDataSo;

		[Header("Load type")]
		[SerializeField] private LevelLoadType loadType;

		[Header("References (Prefab load type)")]
		[SerializeField] private GameObject[] levels;

		[Header("References (AddressableScene load type)")]
		[SerializeField] private AssetReference[] levelScenes;

		private ILevelLoader _levelLoader;
		private CancellationToken _destroyCancellationToken;

		private int _currentLevelIndex;
		private int _currentLevelRating;

		private EventBinding<UIEvents.SelectorItemPlayPressed> _selectorItemPlayPressed;
		private EventBinding<GameEvents.GameStateChanged> _gameStateChanged;
		private EventBinding<UIEvents.UIButtonClicked> _uiButtonClicked;

		private void Awake()
		{
			_destroyCancellationToken = this.GetCancellationTokenOnDestroy();

			_levelLoader = loadType switch
			{
				LevelLoadType.AddressableScene => new AddressableSceneLevelLoader(levelScenes),
				_ => new PrefabLevelLoader(levels)
			};
		}

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

			SpawnLevelAsync(_currentLevelIndex).Forget();
		}

		private void OnGameStateChanged(GameEvents.GameStateChanged eventData)
		{
			if (eventData.State is not GameMainMenuState) return;

			_levelLoader.UnloadCurrentLevel();

			_currentLevelIndex = 0;
			_currentLevelRating = 0;
		}

		private void OnUiButtonClicked(UIEvents.UIButtonClicked eventData)
		{
			if (eventData.ButtonType != UiButtonType.Restart) return;

			SpawnLevelAsync(_currentLevelIndex).Forget();
		}

		private async UniTaskVoid SpawnLevelAsync(int index)
		{
			if (index < 0 || index >= _levelLoader.LevelCount) return;

			await _levelLoader.LoadLevelAsync(index, _destroyCancellationToken);

			if (_destroyCancellationToken.IsCancellationRequested) return;

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