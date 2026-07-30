using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Assets.Scripts.Installers
{
	public class AudioInstaller : MonoInstaller
	{
		[SerializeField] private AudioMixer mixer;

		public override void InstallBindings()
		{
			Container.Bind<AudioMixer>()
				.FromInstance(mixer)
				.AsSingle();
		}
	}
}