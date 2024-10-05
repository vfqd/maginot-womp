using System;
using System.Collections.Generic;
using Game;
using Library;
using Library.Extensions;
using Library.Grid;
using Sirenix.Serialization;
using UnityEngine;

namespace Map
{
    public class MapController : SerializedMonoSingleton<MapController>
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Texture2D mapStartState;
        [SerializeField] private List<TypeColorPair> typeColors;
        [OdinSerialize] private Dictionary<BuildingType, Building> buildingLookup;
        
        public Grid<Tile> Grid => _grid;
        private Grid<Tile> _grid;

        protected override void Awake()
        {
            base.Awake();
            _grid = new Grid<Tile>(mapStartState.width, mapStartState.height,tilePrefab,transform,((tile, x, y) =>
            {
            }));
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    _grid[x, y].Type = GetTypeForColor(mapStartState.GetPixel(x, y));
                }
            }

            foreach (var tile in _grid)
            {
                tile.SetSprite();
                tile.UpdatePathing();
                if (tile.Type == TileType.Ground)
                {
                    var health = 1+Mathf.Pow(Math.Min(1,53-tile.Y),1.2f);
                    tile.tileHealth.SetMaxHp(health);
                }
            }
        }

        private void Start()
        {
            var hub = PlaceBuildingAt(BuildingType.Hub,67,56);
            ResourcesController.Instance.hub = hub;
            hub.AddWomp();
            var digs = PlaceBuildingAt(BuildingType.Diggers,57,56);
            digs.AddWomp();
        }

        public Building PlaceBuildingAt(BuildingType building, int x, int y)
        {
            var prefab = buildingLookup[building];
            var b = Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
            b.center = Grid.GetTile(x, y);
            return b;
        }

        public TileType GetTypeForColor(Color c)
        {
            foreach (var pair in typeColors)
            {
                if (pair.Color.IsEqualTo(c)) return pair.TileType;
            }
            return TileType.Air;
        }
        
        public Color GetColorForType(TileType type)
        {
            foreach (var pair in typeColors)
            {
                if (pair.TileType == type) return pair.Color;
            }
            return Color.clear;
        }
        
        [Serializable]
        public struct TypeColorPair
        {
            public TileType TileType;
            public Color Color;
        }
    }
}
