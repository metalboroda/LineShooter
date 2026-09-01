using Assets.Scripts.Player;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class PlayerInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<PlayerFactory>().AsSingle();
		}
	}
}