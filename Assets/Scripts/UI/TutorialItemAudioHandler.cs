using System.Threading;
using Assets.Scripts.EventBus;
using Assets.Scripts.EventsFolder;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TutorialItemAudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;

        private AudioSource _audioSource;
        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();

            if (audioClip)
            {
                _audioSource.clip = audioClip;
                _audioSource.Play();

                RaiseVoiceoverPlayedEvent(true);

                _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
                
                WaitForClipToEnd(_cts.Token).Forget();
            }
        }

        private void OnDisable()
        {
            RaiseVoiceoverPlayedEvent(false);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTaskVoid WaitForClipToEnd(CancellationToken token)
        {
            float waitTime = audioClip.length;
            float elapsed = 0f;

            while (elapsed < waitTime)
            {
                bool wasCancelled = await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();

                if (wasCancelled) return;

                elapsed += Time.deltaTime;

                if (!_audioSource.isPlaying)
                {
                    RaiseVoiceoverPlayedEvent(false);
                    break;
                }
            }

            RaiseVoiceoverPlayedEvent(false);
        }

        private void RaiseVoiceoverPlayedEvent(bool isVoiceoverPlayed)
        {
            EventBus<AudioEvents.VoiceoverPlayed>.Raise(new AudioEvents.VoiceoverPlayed
            {
                IsVoiceoverPlayed = isVoiceoverPlayed
            });
        }
    }
}
