using System;
using DG.Tweening;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Library.Sprites
{
    public class BounceMoveSprite : MonoBehaviour
    {
        [SerializeField] private float bounceDuration;
        [SerializeField] private float squashAndStretchDuration;
        [SerializeField] private float bounceHeight;
        [SerializeField] private float holdingPause;
        [SerializeField] private float scaleX = 1.2f;
        [SerializeField] private float scaleY = 0.8f;
        [SerializeField] private bool playOnAwake = true;

        private bool _pause;
        private SpriteRenderer _spriteRenderer;
        private Sequence _sequence;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if(playOnAwake) Play();
        }

        [Button]
        public void Toggle()
        {
            if (_sequence != null && _sequence.IsPlaying())
            {
                KillSequence();
            }
            else if (_sequence == null || !_sequence.IsPlaying())
            {
                PlaySequence();
            }
        }

        [Button]
        public void Play()
        {
            PlaySequence();
        }

        [Button]
        public void Stop()
        {
            KillSequence();
        }

        private void PlaySequence()
        {
            KillSequence();
            _sequence = DOTween.Sequence();
            _sequence.Append(_spriteRenderer.transform.DOLocalMoveY(bounceHeight, bounceDuration / 2).SetEase(Ease.OutQuad));
            _sequence.Append(_spriteRenderer.transform.DOLocalMoveY(0, bounceDuration / 2).SetEase(Ease.InQuad));
            _sequence.Append(_spriteRenderer.transform.DOScale(new Vector3(scaleX, scaleY, 1), squashAndStretchDuration / 2).SetEase(Ease.InOutSine));
            _sequence.Append(_spriteRenderer.transform.DOScale(new Vector3(1, 1, 1), squashAndStretchDuration / 2).SetEase(Ease.InOutSine));
            if (holdingPause > 0) _sequence.AppendInterval(holdingPause);
            _sequence.SetLoops(-1);
            _sequence.OnKill(() =>
            {
                _spriteRenderer.transform.localPosition = Vector3.zero;
                _spriteRenderer.transform.SetLossyScale(Vector3.one);
            });
            _sequence.Play();
        }

        private void KillSequence()
        {
            if (_sequence != null)
            {
                _sequence.Kill(true);
                _sequence = null;
            }
        }
    }
}