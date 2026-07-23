using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation.ScaleAnimation
{
    [Serializable]
    public class ScaleFromAnimationConfig
    {
        [field: SerializeField] public GameObject ObjectToMove { get; private set; }
        [field: Space]
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; } = 0.25f;
        [field: Space]
        [field: SerializeField] public Ease Ease { get; private set; } = Ease.OutBack;
    }
}