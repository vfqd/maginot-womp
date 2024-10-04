using System;
using DG.Tweening;
using UnityEngine;

namespace Library.UiAnimation
{
    [Serializable]
    public class EffectShrink : AnimationEffect
    {
        public float duration = 0.1f;
        public float siblingDelay = 0;
        public Ease EaseType = Ease.OutCirc;

        public override void Play(Transform targetTransform, Action completedCallback = null)
        {
            if (IsPlaying) return;
            IsPlaying = true;
            base.completedCallback = completedCallback;
            
            targetTransform.DOKill(true);
            
            targetTransform
                .DOScale(Vector3.zero, duration)
                .SetEase(EaseType)
                .SetUpdate(true)
                .SetDelay(siblingDelay)
                .OnComplete(() =>
                {
                    FinishPlaying();
                    targetTransform.localScale = Vector3.zero; 
                });
        }

        public override float TotalDuration()
        {
            return duration;
        }
    }
}