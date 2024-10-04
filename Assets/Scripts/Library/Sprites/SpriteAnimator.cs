using System;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Library.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField,InlineEditor()] private SpriteAnimation defaultAnim;
        [SerializeField] private bool playOnAwake;
        [SerializeField] private float animSpeedMultiplier = 1;
        
        [ShowInInspector] public SpriteAnimation ActiveAnimation => _activeAnimation;
        
        private SpriteAnimation _activeAnimation;
        private float _frameStartTime;
        private int _frameIndex;
        private Action _callbackAction;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (playOnAwake && defaultAnim != null) Play();
        }

        public void Update()
        {
            if (!_activeAnimation) return;
            if (_frameIndex < 0 || _frameIndex >= _activeAnimation.frames.Length) return;
            
            float now = Time.time;
            if (now >= _frameStartTime + (_activeAnimation.frames[_frameIndex].duration / animSpeedMultiplier))
            {
                PlayNextFrame();
            }
        }

        private void PlayNextFrame()
        {
            if (_activeAnimation.loop)
            {
                _frameIndex = MathUtils.WrapIndex(_frameIndex + 1, _activeAnimation.frames.Length);
            }
            else
            {
                _frameIndex++;
            }

            if (_frameIndex >= _activeAnimation.frames.Length)
            {
                enabled = false;
                return;
            }
            
            _frameStartTime = Time.time;
            _spriteRenderer.sprite = _activeAnimation.frames[_frameIndex].sprite;
            
            if (_activeAnimation.frames[_frameIndex].hasEvent)
            {
                _callbackAction?.Invoke();
                // var className = _activeAnimation.frames[_frameIndex].eventClassName;
                // var methodName = _activeAnimation.frames[_frameIndex].animationEvent;
                // var script = GetComponent(className);
                // if (script != null && script is MonoBehaviour monoBehaviour)
                // {
                //     monoBehaviour.Invoke(methodName,0);
                // }
            }
        }


        // Public 


        public void Play()
        {
            Play(defaultAnim);
        }

        public void Play(SpriteAnimation anim, float animSpeed = 1, Action callbackAction = null)
        {
            enabled = true;
            _frameIndex = anim.GetStartIndex();
            _frameStartTime = Time.time + anim.GetStartTimeOffset(_frameIndex);
            animSpeedMultiplier = animSpeed;
            _callbackAction = callbackAction;
            _activeAnimation = anim;
            _spriteRenderer.sprite = _activeAnimation.frames[_frameIndex].sprite;
        }

        public void Stop()
        {
            enabled = false;
            _activeAnimation = null;
        }

        public void SetAnimMultiplier(float multiplier)
        {
            animSpeedMultiplier = multiplier;
        }

        /*[Button,HideIf("defaultAnim")]
        private void CreateAnonymousDefaultAnim()
        {
            defaultAnim = ScriptableObject.CreateInstance<SpriteAnimation>();
            playOnAwake = true;
        }*/
    }
}