using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameManagement.LevelLoading
{
	public class PrefabLevelLoader : ILevelLoader
	{
		private readonly GameObject[] _levelPrefabs;

		private GameObject _spawnedLevel;

		public PrefabLevelLoader(GameObject[] levelPrefabs)
		{
			_levelPrefabs = levelPrefabs;
		}

		public int LevelCount => _levelPrefabs?.Length ?? 0;
		public bool HasLoadedLevel => _spawnedLevel is not null;

		public UniTask LoadLevelAsync(int index, CancellationToken token)
		{
			UnloadCurrentLevel();

			_spawnedLevel = Object.Instantiate(_levelPrefabs[index]);

			return UniTask.CompletedTask;
		}

		public void UnloadCurrentLevel()
		{
			if (_spawnedLevel is null) return;

			Object.Destroy(_spawnedLevel);
			
			_spawnedLevel = null;
		}
	}
}