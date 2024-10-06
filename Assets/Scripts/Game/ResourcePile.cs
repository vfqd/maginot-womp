using System;
using DG.Tweening;
using Map;
using UnityEngine;

namespace Game
{
    public class ResourcePile : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public ResourceType type;
        public float value;
        public bool canBeCollected = false;
        public bool pickedUp = false;

        private Sequence _moveDown;
        private float _ttl = 60;

        private void Update()
        {
            if (pickedUp) return;
            if (_moveDown != null) return;
            var position = transform.position;
            var currentTile = MapController.Instance.Grid.GetTile(position.x, position.y);
            if (currentTile && !currentTile.CanBeStoodOn())
            {
                _moveDown = DOTween.Sequence();
                _moveDown.Append(transform.DOMoveY(transform.position.y - 1, 0.5f).SetEase(Ease.InOutSine));
                _moveDown.OnComplete(() => _moveDown = null);
                _moveDown.Play();
            }

            _ttl -= Time.deltaTime;
            if (transform.position.y < 0)
            {
                ResourcesController.Instance.DestroyPile(this);
            }
        }
    }

    public enum ResourceType
    {
        Sand,
        Plastic,
        Metal,
        Womps,
        Archers,
        Soldiers,
        Artillerymen,
        Research
    }
}