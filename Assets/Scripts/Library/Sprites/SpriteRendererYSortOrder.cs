using UnityEngine;

namespace Library.Sprites
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererYSortOrder : MonoBehaviour
    {
        private const int MaxIndex = 5000;
        private const float DivisionCount = 0.1f;

        public bool changeOnUpdate;
        public int offset;

        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            if (!changeOnUpdate) return;
            UpdateSpriteOrder();
        }

        public void UpdateSpriteOrder()
        {
            var round = Mathf.RoundToInt(transform.position.y / DivisionCount);
            _spriteRenderer.sortingOrder = MaxIndex - round + offset;
        }
    }
}