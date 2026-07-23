using System.Threading;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundtrackPlayer : MonoBehaviour
	{
		[Header("Reducing By Voice")]
		[SerializeField] private float reducedVolume = 0.2f;
		[SerializeField] private float volumeChangeDuration = 0.125f;

		[Header("Clips")]
		[SerializeField] private AudioClip[] soundtrackClips;

		[Header("References")]
		[SerializeField] private AudioSource audioSource;

		private float _initVolume;
		private CancellationTokenSource _volumeChangeCts;

		private EventBinding<AudioEvents.VoiceoverPlayed> _voiceoverPlayed;

		private void OnEnable()
		{
			_voiceoverPlayed = new EventBinding<AudioEvents.VoiceoverPlayed>(OnVoiceoverPlayed);
			EventBus<AudioEvents.VoiceoverPlayed>.Register(_voiceoverPlayed);
		}

		private void OnDisable()
		{
			EventBus<AudioEvents.VoiceoverPlayed>.Unregister(_voiceoverPlayed);
		}

		private void Start()
		{
			if (!audioSource)
			{
				Debug.LogError("Audio Source must be assigned!");

				enabled = false;
				return;
			}

			_initVolume = audioSource.volume;

			DoPlaySoundtracks(this.GetCancellationTokenOnDestroy()).Forget();
		}

		private async UniTaskVoid DoPlaySoundtracks(CancellationToken token)
		{
			if (soundtrackClips == null || soundtrackClips.Length == 0)
			{
				Debug.LogWarning("Soundtrack clips array is empty. Stopping playback.");
				return;
			}

			while (true)
			{
				int startIndex = Random.Range(0, soundtrackClips.Length);

				for (int i = 0; i < soundtrackClips.Length; i++)
				{
					int currentClipIndex = (startIndex + i) % soundtrackClips.Length;

					AudioClip clipToPlay = soundtrackClips[currentClipIndex];

					if (!clipToPlay) continue;

					audioSource.clip = clipToPlay;
					audioSource.Play();

					bool wasCancelled = await UniTask.WaitWhile(() => audioSource.isPlaying, cancellationToken: token)
						.SuppressCancellationThrow();

					if (wasCancelled) return;
				}
			}
		}

		private void OnVoiceoverPlayed(AudioEvents.VoiceoverPlayed eventData)
		{
			StopVolumeChangeInternal();

			float targetVolume = eventData.IsVoiceoverPlayed ? reducedVolume : _initVolume;

			if (audioSource && gameObject.activeInHierarchy)
			{
				_volumeChangeCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
				
				ChangeVolume(targetVolume, volumeChangeDuration, _volumeChangeCts.Token).Forget();
			}
		}

		private async UniTaskVoid ChangeVolume(float targetVolume, float duration, CancellationToken token)
		{
			float currentTime = 0;
			float startVolume = audioSource.volume;

			while (currentTime < duration)
			{
				currentTime += Time.unscaledDeltaTime;

				audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);

				bool wasCancelled = await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();

				if (wasCancelled) return;
			}

			audioSource.volume = targetVolume;

			_volumeChangeCts = null;
		}

		private void StopVolumeChangeInternal()
		{
			if (_volumeChangeCts != null)
			{
				_volumeChangeCts.Cancel();
				_volumeChangeCts.Dispose();
				_volumeChangeCts = null;
			}
		}
	}
}
