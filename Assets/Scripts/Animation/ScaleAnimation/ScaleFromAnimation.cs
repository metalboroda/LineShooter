using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation.ScaleAnimation
{
    public class ScaleFromAnimation : MonoBehaviour
    {
        [SerializeField] private float globalDelay = 0.1f;
        [Space]
        [SerializeField] private List<ScaleFromAnimationConfig> animationConfigs;

        private readonly Dictionary<GameObject, Vector3> _originalScales = new();
        private bool _isInitialized;

        private void OnEnable()
        {
            Animate();
        }

        private void InitializeOriginalScales()
        {
            _originalScales.Clear();

            foreach (var config in animationConfigs)
            {
                if (config.ObjectToMove && !_originalScales.ContainsKey(config.ObjectToMove))
                    _originalScales[config.ObjectToMove] = config.ObjectToMove.transform.localScale;
            }
        }

        public void Animate()
        {
            if (!_isInitialized)
            {
                InitializeOriginalScales();
                
                _isInitialized = true;
            }

            ResetAllToFromState();

            DOVirtual.DelayedCall(globalDelay, () =>
            {
                foreach (var config in animationConfigs)
                {
                    PlaySingleAnimation(config);
                }
            });
        }

        private void ResetAllToFromState()
        {
            foreach (var config in animationConfigs)
            {
                if (config.ObjectToMove)
                    config.ObjectToMove.transform.localScale = Vector3.zero;
            }
        }

        private void PlaySingleAnimation(ScaleFromAnimationConfig config)
        {
            if (!config.ObjectToMove || !_originalScales.ContainsKey(config.ObjectToMove)) return;

            Vector3 originalScale = _originalScales[config.ObjectToMove];

            config.ObjectToMove.transform
                .DOScale(originalScale, config.Duration)
                .SetDelay(config.Delay)
                .SetEase(config.Ease)
                .SetUpdate(true);
        }
    }
}