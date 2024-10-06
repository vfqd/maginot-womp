using System;
using Framework;
using Game;
using Library.Extensions;
using Library.Grid;
using Pathfinding;
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
        [PreviewField] public Sprite[] metalSprites;
        [PreviewField] public Sprite ladderSprite;
        [PreviewField] public GameObject upEdge;
        [PreviewField] public GameObject downEdge;
        [PreviewField] public GameObject leftEdge;
        [PreviewField] public GameObject rightEdge;
        public GameObject bg;
        public GameObject col;

        public TileHealth tileHealth;
        
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
            tileHealth = GetComponent<TileHealth>();
        }

        private void Update()
        {
            if (Type == TileType.Ocean && _spriteRenderer.sprite != null)
            {
                transform.SetLocalY(Y+Mathf.Sin(_offset+Time.time*3)*0.1f);
            }
        }

        public bool CanBeStoodOn()
        {
            if (Type.IsSolid() && Type != TileType.Ladder) return false;
            var belowTile = this.DownNeighbour();
            if (belowTile && belowTile.Type.IsNotSolid() && belowTile.Type != TileType.Ladder)
            {
                return false;
            };
            return true;
        }

        public Tile GetNeighbourThatCanBeStoodOn()
        {
            var down = this.DownNeighbour();
            if (down && down.CanBeStoodOn()) return down;
            foreach (var n in this.Get8NeighboursEnumerable())
            {
                if (n.CanBeStoodOn())
                {
                    return n;
                }
            }
            return null;
        }

        public void UpdatePathing()
        {
            col.SetActive(Type is TileType.Ground or TileType.Sandcastle);

            var walkability = false;
            var canSwim = MapController.Instance.canSwimInSea > 0.5f;
            if (Type == TileType.Ladder) walkability = true;
            else if (canSwim && Type == TileType.Ocean) walkability = true;
            else if (Type is TileType.Air or TileType.Room)
            {
                if (this.DownNeighbour()?.Type.IsSolid() != false)
                    walkability = true;
                else if (canSwim && this.DownNeighbour()?.Type is TileType.Ocean && this.DownRightNeighbour()?.Type is TileType.Ground)
                    walkability = true;
                
                else if (this.DownNeighbour()?.Type == TileType.Air)
                {
                    if (this.DownNeighbour().DownNeighbour()?.Type == TileType.Air)
                        walkability = false;
                    else if (this.DownLeftNeighbour()?.Type.IsSolid() != false)
                        walkability = true;
                    else if (this.DownRightNeighbour()?.Type.IsSolid() != false)
                        walkability = true;
                }
            }
            else if (Type == TileType.Sandcastle)
            {
                if (this.DownNeighbour().Type is TileType.Ground)
                {
                    walkability = true;
                }
            }

            GraphUpdateObject guo = new GraphUpdateObject(new Bounds(transform.position,Vector3.one))
            {
                modifyWalkability = true,
                setWalkability = walkability
            };
            AstarPath.active.UpdateGraphs(guo);
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
                case TileType.Metal:
                    _spriteRenderer.sprite = metalSprites.GetRandomElement();
                    break;
                case TileType.Ladder:
                    _spriteRenderer.sprite = ladderSprite;
                    break;
                case TileType.Ocean:
                    if (Random.value < 0.05f)
                    {
                        if (this.UpNeighbour()?.Type == TileType.Air) break;
                        if (this.UpNeighbour()?.UpNeighbour()?.Type == TileType.Air) break;
                        _spriteRenderer.sprite = oceanSprites.GetRandomElement();
                    }
                    break;
                default:
                    _spriteRenderer.sprite = null;
                    break;
            }

            if (Type.IsSolid() && Type != TileType.Ladder)
            {
                upEdge.SetActive(this.UpNeighbour()?.Type.IsNotSolid() != false);
                downEdge.SetActive(this.DownNeighbour()?.Type.IsNotSolid() != false);
                leftEdge.SetActive(this.LeftNeighbour()?.Type.IsNotSolid() != false);
                rightEdge.SetActive(this.RightNeighbour()?.Type.IsNotSolid() != false);
            }
            else
            {
                upEdge.SetActive(false);
                downEdge.SetActive(false);
                leftEdge.SetActive(false);
                rightEdge.SetActive(false);
            }
            
            bg.SetActive(Type is TileType.Ground or TileType.Metal);
        }

        public void ChangeTileTo(TileType type)
        {
            if (Type == TileType.Ground && type == TileType.Air)
            {
                ResourcesController.Instance.CreateResourcePileAt(ResourceType.Sand,transform.position, Mathf.Max(1,58 - Y),true);
            }
            else if (Type == TileType.Sandcastle && type == TileType.Air)
            {
                ResourcesController.Instance.CreateResourcePileAt(ResourceType.Sand,transform.position, 1,true);
            }
            else if (Type == TileType.Metal && type == TileType.Air)
            {
                ResourcesController.Instance.CreateResourcePileAt(ResourceType.Metal,transform.position, Mathf.Max(1,40 - Y),true);
            }
            
            Type = type;
            SetSprite();
            UpdatePathing();

            foreach (var neighbour in this.Get8NeighboursEnumerable())
            {
                neighbour.SetSprite();
                neighbour.UpdatePathing();
            }
                        
            var up = this.UpNeighbour();
            if (up && up.Type.IsNotSolid())
            {
                foreach (var neighbour in up.Get4NeighboursEnumerable())
                {
                    neighbour.SetSprite();
                    neighbour.UpdatePathing();
                }
            }
        }
    }

    public enum TileType
    {
        Air,
        Ground,
        Ocean,
        Sandcastle,
        Ladder,
        Room,
        Metal
    }

    public static class TypeExtensions
    {
        public static bool IsSolid(this TileType type)
        {
            return type is TileType.Ground or TileType.Sandcastle or TileType.Ladder or TileType.Metal;
        }
        
        public static bool IsNotSolid(this TileType type)
        {
            return type is TileType.Air or TileType.Ladder or TileType.Room or TileType.Ocean;
        }
    }

}