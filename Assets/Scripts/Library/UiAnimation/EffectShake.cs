using System;
using DG.Tweening;
using UnityEngine;

namespace Library.UiAnimation
{
    [Serializable]
    public class EffectShake : AnimationEffect
    {
        public float shakeDuration = 0.4f;
        public float rotateDuration = 0.1f;
        public float strength = 5f;
        public int vibrato = 20;
        public Ease ease = Ease.InOutBounce;

        public override void Play(Transform targetTransform, Action completedCallback = null)
        {
            if (IsPlaying) return;
            IsPlaying = true;
            base.completedCallback = completedCallback;
            
            targetTransform.DOKill(true);
            Sequence anim = DOTween.Sequence();
            anim.Append(targetTransform.DOShakeRotation(shakeDuration, strength, vibrato).SetEase(ease).SetUpdate(true));
            anim.Append(targetTransform.DOLocalRotate(Vector3.zero, rotateDuration));
            anim.OnComplete(FinishPlaying);
            anim.Play();
        }

        public override float TotalDuration()
        {
            return shakeDuration + rotateDuration;
        }
    }
}