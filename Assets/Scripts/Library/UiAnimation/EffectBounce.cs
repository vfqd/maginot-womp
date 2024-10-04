using System;
using DG.Tweening;
using UnityEngine;

namespace Library.UiAnimation
{
    [Serializable]
    public class EffectBounce : AnimationEffect
    {
        public Vector3 punchScale = new (0.05f, 0.05f);
        public float duration = 0.25f;
        public Ease ease = Ease.InOutBounce;

        public override void Play(Transform targetTransform, Action completedCallback = null)
        {
            if (IsPlaying) return;
            IsPlaying = true;
            base.completedCallback = completedCallback;

            targetTransform.DOKill(true);
            targetTransform
                .DOPunchScale(punchScale, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    targetTransform.localScale = Vector3.one;
                    FinishPlaying();
                });
        }

        public override float TotalDuration()
        {
            return duration;
        }
    }
}