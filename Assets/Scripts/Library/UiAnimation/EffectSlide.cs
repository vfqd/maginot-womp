using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Library.UiAnimation
{
    [Serializable]
    public class EffectSlide : AnimationEffect
    {
        public Vector2 finalLocalPosition;
        public float duration = 0.25f;
        public Ease ease = Ease.InOutSine;

        public override void Play(Transform targetTransform, Action completedCallback = null)
        {
            if (IsPlaying) return;
            IsPlaying = true;
            base.completedCallback = completedCallback;

            targetTransform.DOKill(true);
            targetTransform
                .DOLocalMove(finalLocalPosition, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    targetTransform.localPosition = finalLocalPosition;
                    FinishPlaying();
                });
        }

        public override float TotalDuration()
        {
            return duration;
        }
    }
}