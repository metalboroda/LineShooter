using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Assets.Scripts.Player;

namespace Assets.Scripts.LevelManagement
{
	public class LevelHandler : MonoBehaviour
	{
		[Inject] private PlayerFactory _playerFactory;
		
		[Header("References")]
		[SerializeField] private AssetReference playerPrefab;
		[Space]
		[SerializeField] private Transform playerSpawnPoint;

		private PlayerController _player;

		private async void Start()
		{
			await SpawnPlayerAsync(this.GetCancellationTokenOnDestroy());
		}

		private async UniTask SpawnPlayerAsync(CancellationToken token)
		{
			if (_playerFactory == null)
			{
				Debug.LogError("[LevelHandler] PlayerFactory не заінжектився. Перевір SceneContext у цій сцені та Installers.");
				return;
			}

			if (!playerSpawnPoint)
			{
				Debug.LogError("[LevelHandler] playerSpawnPoint не призначений в інспекторі.");
				return;
			}

			if (!playerPrefab.RuntimeKeyIsValid())
			{
				Debug.LogError("[LevelHandler] playerPrefab не призначений або має невалідний ключ Addressables.");
				return;
			}

			_player = await _playerFactory.SpawnAsync(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, token);
		}
	}
}