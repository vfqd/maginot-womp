using System;
using UnityEngine;

namespace Library.UiAnimation
{
    [Serializable]
    public abstract class AnimationEffect
    {
        public bool IsPlaying;
        protected Action completedCallback;

        public abstract void Play(Transform targetTransform, Action completedCallback = null);

        public abstract float TotalDuration();

        public virtual void FinishPlaying()
        {
            IsPlaying = false;
            completedCallback?.Invoke();
        }
    }
}