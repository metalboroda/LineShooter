using Assets.Scripts.ScriptableObjects.Infrastructure;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class DataInstaller : MonoInstaller
	{
		[SerializeField] private CoinDataSo coinDataSo;
		[SerializeField] private LevelDataSo levelDataSo;

		public override void InstallBindings()
		{
			Container.Bind<CoinDataSo>()
				.FromInstance(coinDataSo)
				.AsSingle();

			Container.Bind<LevelDataSo>()
				.FromInstance(levelDataSo)
				.AsSingle();
		}
	}
}