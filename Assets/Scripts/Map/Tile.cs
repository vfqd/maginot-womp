using System;
using Framework;
using Library.Extensions;
using Library.Grid;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class Tile : MonoBehaviour, ITile
    {
        [PreviewField] public Sprite[] groundSprites;
        [PreviewField] public Sprite[] oceanSprites;
        [PreviewField] public Sprite[] sandcastleSprites;
        [PreviewField] public GameObject upEdge;
        [PreviewField] public GameObject downEdge;
        [PreviewField] public GameObject leftEdge;
        [PreviewField] public GameObject rightEdge;
        
        private SpriteRenderer _spriteRenderer;
        public TileType Type;
        
        private Grid<Tile> _grid;
        public int X { get; set; }
        public int Y { get; set; }

        private float _offset;

        public Grid<T> GetGrid<T>() where T : ITile, new()
        {
            _grid ??= MapController.Instance.Grid;
            return _grid as Grid<T>;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _offset = Random.Range(0, 5f);
        }

        private void Update()
        {
            if (Type == TileType.Ocean && _spriteRenderer.sprite != null)
            {
                transform.SetLocalY(Y+Mathf.Sin(_offset+Time.time*3)*0.1f);
            }
        }

        public void SetSprite()
        {
            switch (Type)
            {
                case TileType.Ground:
                    if (Random.value < 0.1f)
                    {
                        _spriteRenderer.sprite = groundSprites.GetRandomElement();
                    }
                    break;
                case TileType.Sandcastle:
                    _spriteRenderer.sprite = sandcastleSprites.GetRandomElement();
                    break;
                case TileType.Ocean:
                    if (Random.value < 0.05f)
                    {
                        if (this.UpNeighbour()?.Type == TileType.Air) break;
                        if (this.UpNeighbour()?.UpNeighbour()?.Type == TileType.Air) break;
                        _spriteRenderer.sprite = oceanSprites.GetRandomElement();
                    }
                    break;
            }

            if (Type == TileType.Ground || Type == TileType.Sandcastle)
            {
                upEdge.SetActive(this.UpNeighbour()?.Type != Type);
                downEdge.SetActive(this.DownNeighbour()?.Type != Type);
                leftEdge.SetActive(this.LeftNeighbour()?.Type != Type);
                rightEdge.SetActive(this.RightNeighbour()?.Type != Type);
            }
            else
            {
                upEdge.SetActive(false);
                downEdge.SetActive(false);
                leftEdge.SetActive(false);
                rightEdge.SetActive(false);
            }
        }
    }

    public enum TileType
    {
        Air,
        Ground,
        Ocean,
        Sandcastle
    }

}