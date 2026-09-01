using Assets.Scripts.Input;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class InputInstaller : MonoInstaller
	{
		[SerializeField] private InputReader inputReader;

		public override void InstallBindings()
		{
			Container.Bind<IMovementInput>()
				.FromInstance(inputReader)
				.AsSingle();
		}
	}
}