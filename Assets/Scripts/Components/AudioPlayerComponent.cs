using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Components
{
    public class AudioPlayerComponent
    {
        private CancellationTokenSource _currentCts;
        
        private AudioSource _audioSource;
        private MonoBehaviour _monoBehaviour;

        public AudioPlayerComponent SetAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
            return this;
        }

        public AudioPlayerComponent SetMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
            return this;
        }

        public void PlayClip(AudioClip clip, bool oneShot = false, bool randomPitch = false, float? delay = null)
        {
            if (!clip || !_audioSource) return;
            if (delay.HasValue)
            {
                if (!_monoBehaviour) return;

                StopCurrentTask();

                _currentCts = CancellationTokenSource.CreateLinkedTokenSource(_monoBehaviour.GetCancellationTokenOnDestroy());
                
                PlayWithDelay(clip, delay.Value, oneShot, randomPitch, _currentCts.Token).Forget();
            }
            else
            {
                if (randomPitch) SetRandomPitch();

                PlayAudioClip(clip, oneShot);
            }
        }

        public void PlayClips(AudioClip[] clips, bool oneShot = false, bool randomPitch = false, float? delay = null)
        {
            if (clips.Length == 0 || !_audioSource) return;
            if (delay.HasValue)
            {
                if (!_monoBehaviour) return;

                StopCurrentTask();

                _currentCts = CancellationTokenSource.CreateLinkedTokenSource(_monoBehaviour.GetCancellationTokenOnDestroy());
                
                PlayMultipleWithDelay(clips, delay.Value, oneShot, randomPitch, _currentCts.Token).Forget();
            }
            else
            {
                foreach (AudioClip clip in clips)
                {
                    if (clip)
                    {
                        if (randomPitch) SetRandomPitch();

                        PlayAudioClip(clip, oneShot);
                    }
                }
            }
        }

        public void PlayRandomClip(AudioClip[] clips, bool oneShot = false, bool randomPitch = false, float? delay = null)
        {
            if (clips.Length == 0 || !_audioSource) return;

            AudioClip randomClip = clips[Random.Range(0, clips.Length)];

            PlayClip(randomClip, oneShot, randomPitch, delay);
        }

        public void StopAll()
        {
            StopCurrentTask();

            if (_audioSource?.isPlaying == true)
            {
                _audioSource.Stop();
            }
        }

        private void StopCurrentTask()
        {
            if (_currentCts != null)
            {
                _currentCts.Cancel();
                _currentCts.Dispose();

                _currentCts = null;
            }
        }

        private void SetRandomPitch(float minPitch = 0.95f, float maxPitch = 1.05f)
        {
            if (_audioSource)
                _audioSource.pitch = Random.Range(minPitch, maxPitch);
        }

        private void PlayAudioClip(AudioClip clip, bool oneShot)
        {
            if (oneShot)
            {
                _audioSource.PlayOneShot(clip);
            }
            else
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }

        private async UniTaskVoid PlayWithDelay(AudioClip clip, float delay, bool oneShot, bool randomPitch, CancellationToken token)
        {
            bool wasCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token)
                .SuppressCancellationThrow();

            if (wasCancelled) return;

            if (randomPitch) SetRandomPitch();

            PlayAudioClip(clip, oneShot);

            _currentCts?.Dispose();
            _currentCts = null;
        }

        private async UniTaskVoid PlayMultipleWithDelay(AudioClip[] clips, float delay, bool oneShot, bool randomPitch, CancellationToken token)
        {
            bool wasCancelled = await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token)
                .SuppressCancellationThrow();

            if (wasCancelled) return;

            foreach (AudioClip clip in clips)
            {
                if (clip)
                {
                    if (randomPitch) SetRandomPitch();

                    PlayAudioClip(clip, oneShot);
                }
            }

            _currentCts?.Dispose();
            _currentCts = null;
        }
    }
}
