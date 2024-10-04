using System;
using UnityEngine;

namespace Library.Sprites
{
    public class PingPong : MonoBehaviour
    {
        [SerializeField] private float deltaX;
        [SerializeField] private float speed;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Vector3 _startPos;

        private void Awake()
        {
            _startPos = transform.position;
        }

        private void Update()
        {
            if (spriteRenderer.flipX == false)
            {
                if (transform.position.x < _startPos.x + deltaX)
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0);
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
            else
            {
                if (transform.position.x > _startPos.x - deltaX)
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0);
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
    }
}