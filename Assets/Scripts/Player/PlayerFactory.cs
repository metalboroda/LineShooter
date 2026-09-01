using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Assets.Scripts.Player
{
	public class PlayerFactory
	{
		private readonly DiContainer _container;

		public PlayerFactory(DiContainer container)
		{
			_container = container;
		}

		public async UniTask<PlayerController> SpawnAsync(AssetReference playerPrefabReference, Vector3 position, Quaternion rotation, CancellationToken token = default)
		{
			AsyncOperationHandle<GameObject> handle = playerPrefabReference.LoadAssetAsync<GameObject>();
			GameObject prefab = await handle.ToUniTask(cancellationToken: token);
			GameObject instance = _container.InstantiatePrefab(prefab, position, rotation, null);
			Addressables.Release(handle);

			return instance.GetComponent<PlayerController>();
		}
	}
}