using Assets.Scripts.GameManagement;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class AudioInstaller : MonoInstaller
	{
		[SerializeField] private AudioMixer mixer;
		[Space]
		[SerializeField] private AudioManager audioManager;

		public override void InstallBindings()
		{
			Container.Bind<AudioMixer>()
				.FromInstance(mixer)
				.AsSingle();

			Container.BindInterfacesAndSelfTo<AudioManager>()
				.FromInstance(audioManager)
				.AsSingle();
		}
	}
}