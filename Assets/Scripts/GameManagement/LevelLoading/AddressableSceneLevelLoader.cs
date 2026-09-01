using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameManagement.LevelLoading
{
	public class AddressableSceneLevelLoader : ILevelLoader
	{
		private readonly AssetReference[] _levelScenes;

		private AsyncOperationHandle<SceneInstance> _sceneHandle;
		private bool _hasLoadedLevel;

		public AddressableSceneLevelLoader(AssetReference[] levelScenes)
		{
			_levelScenes = levelScenes;
		}

		public int LevelCount => _levelScenes?.Length ?? 0;
		public bool HasLoadedLevel => _hasLoadedLevel;

		public async UniTask LoadLevelAsync(int index, CancellationToken token)
		{
			UnloadCurrentLevel();

			_sceneHandle = Addressables.LoadSceneAsync(_levelScenes[index], LoadSceneMode.Additive);

			await UniTask.WaitUntil(() => _sceneHandle.IsDone, cancellationToken: token);

			if (token.IsCancellationRequested) return;

			if (_sceneHandle.Status != AsyncOperationStatus.Succeeded)
			{
				Debug.LogError($"[AddressableSceneLevelLoader] Failed to load level scene at index {index}");
				return;
			}

			_hasLoadedLevel = true;
		}

		public void UnloadCurrentLevel()
		{
			if (!_hasLoadedLevel) return;

			Addressables.UnloadSceneAsync(_sceneHandle);

			_hasLoadedLevel = false;
		}
	}
}