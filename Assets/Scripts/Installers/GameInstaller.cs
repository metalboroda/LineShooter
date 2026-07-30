using Assets.Scripts.GameManagement;
using Assets.Scripts.SaveSystem;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class GameInstaller : MonoInstaller
	{
		[SerializeField] private CoinManager coinManager;

		public override void InstallBindings()
		{
			Container.Bind<ICoinWallet>()
				.FromInstance(coinManager)
				.AsSingle();

			Container.Bind<ISaveService>()
				.To<SaveService>()
				.AsSingle();
		}
	}
}