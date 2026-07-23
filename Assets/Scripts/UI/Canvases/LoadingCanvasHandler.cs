using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Canvases
{
    public class LoadingCanvasHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string gameSceneName;
        [Space]
        [SerializeField] private float gameSceneDelay = 0.01f;

        private void Awake()
        {
            DoLoadGameScene(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid DoLoadGameScene(CancellationToken token)
        {
            bool wasCancelled = await UniTask.Delay(TimeSpan.FromSeconds(gameSceneDelay), cancellationToken: token)
                .SuppressCancellationThrow();

            if (wasCancelled) return;

            SceneManager.LoadScene(gameSceneName);
        }
    }
}
