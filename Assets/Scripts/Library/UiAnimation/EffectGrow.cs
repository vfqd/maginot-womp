using System;
using DG.Tweening;
using UnityEngine;

namespace Library.UiAnimation
{
    [Serializable]
    public class EffectGrow : AnimationEffect
    {
        public float duration = 0.5f;
        public float overshoot = 1.70158f;
        public float targetScale = 1;
        public float siblingDelay = 0;
        public Ease EaseType = Ease.OutBack;

        public override void Play(Transform targetTransform, Action completedCallback = null)
        {
            if (IsPlaying) return;
            IsPlaying = true;
            base.completedCallback = completedCallback;
            
            targetTransform.DOKill(true);
            targetTransform.localScale = Vector3.one * 0.01f;

            targetTransform
                .DOScale(Vector3.one * targetScale, duration)
                .SetEase(EaseType,overshoot)
                .SetUpdate(true)
                .SetDelay(targetTransform.GetSiblingIndex() * siblingDelay)
                .OnComplete(() =>
                {
                    targetTransform.localScale = Vector3.one * targetScale;
                    FinishPlaying();
                });
        }

        public override float TotalDuration()
        {
            return duration;
        }
    }
}