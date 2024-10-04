using System;
using Framework;
using UnityEngine;

namespace Library.Sprites
{
    public class SquashAndStretchSprite : MonoBehaviour
    {
        [SerializeField] private float scaleX = 1.05f;
        [SerializeField] private float scaleY = 0.95f;
        [SerializeField] private float speed = 1;

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var sine = MathUtils.NormalizedSin(Time.time * speed);
            var x = Mathf.Lerp(1, scaleX, sine);
            var y = Mathf.Lerp(1, scaleY, sine);
            _spriteRenderer.transform.localScale = new Vector3(x, y, 1);
        }
    }
}