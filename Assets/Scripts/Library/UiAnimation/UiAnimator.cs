using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Library.UiAnimation
{
    public class UiAnimator : SerializedMonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private bool showByDefault = true;
        [OdinSerialize] private AnimationEffect onMouseEnterEffect;
        [OdinSerialize] private AnimationEffect onHoverEffect;
        [OdinSerialize] private AnimationEffect onClickEffect;
        [OdinSerialize] private AnimationEffect errorEffect;
        
        [OdinSerialize] private AnimationEffect showEffect;
        [OdinSerialize] private AnimationEffect hideEffect;

        public Button button;

        public bool IsShown => _shown;
        private bool _isHovered = false;
        private bool _shown = false;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
            if (button && button.interactable == false)
            {
                return;
            }
            
            if (onMouseEnterEffect != null)
            {
                targetTransform.DOKill(true);
                onMouseEnterEffect?.Play(targetTransform);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _isHovered = false;
            if (button && button.interactable == false)
            {
                if (errorEffect != null)
                {
                    targetTransform.DOKill(true);
                    errorEffect?.Play(targetTransform);
                }
                return;
            }
            
            if (onClickEffect != null)
            {
                targetTransform.DOKill(true);
                onClickEffect?.Play(targetTransform);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
        }

        [Button]
        public void Show()
        {
            // if (_shown) return;
            // _shown = true;
            
            targetTransform.gameObject.SetActive(true);
            if (showEffect != null)
            {
                targetTransform.DOKill();
                showEffect?.Play(targetTransform, () =>
                {
                    targetTransform.gameObject.SetActive(true);
                });
            }
        }

        [Button]
        public void Hide()
        {
            Hide(false, null);
        }
        
        public void Hide(Action onCompletedCallback)
        {
            Hide(false,onCompletedCallback);
        }
        
        private void Hide(bool immediate, Action onCompletedCallback)
        {
            // if (!_shown) return;
            // _shown = false;
            
            if (immediate || hideEffect == null)
            {
                targetTransform.gameObject.SetActive(false);
                return;
            }
            
            targetTransform.DOKill();
            hideEffect?.Play(targetTransform,()=>
            {
                onCompletedCallback.Invoke();
                targetTransform.gameObject.SetActive(false);
            });
        }

        public void BounceOneShot(Action completedCallback = null)
        {
            var bounce = new EffectBounce
            {
                IsPlaying = false
            };
            bounce.Play(targetTransform,completedCallback);
        }
        
        public void ShakeOneShot(Action completedCallback = null)
        {
            var shake = new EffectShake();
            shake.Play(targetTransform,completedCallback);
        }

        private void OnEnable()
        {
            if(showByDefault) Show();
        }

        private void Update()
        {
            if (_isHovered)
            {
                onHoverEffect?.Play(targetTransform);
            }
        }
    }
}