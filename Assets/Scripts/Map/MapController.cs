using System;
using System.Collections.Generic;
using Game;
using Library;
using Library.Extensions;
using Library.Grid;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Map
{
    public class MapController : SerializedMonoSingleton<MapController>
    {
        [SerializeField] private Tile tilePrefab;
        [SerializeField] private Texture2D mapStartState;
        [SerializeField] private List<TypeColorPair> typeColors;
        [OdinSerialize] public Dictionary<BuildingType, Building> buildingLookup;

        public List<Building> buildings;
        public FloatParameter canSwimInSea;
        
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
            buildings = new List<Building>();

            foreach (var tile in _grid)
            {
                tile.SetSprite();
                tile.UpdatePathing();
                if (tile.Type == TileType.Ground)
                {
                    var health = 1+Mathf.Pow(Math.Max(1,53-tile.Y),1.2f);
                    tile.tileHealth.SetMaxHp(health);
                }
            }
        }

        public Building PlaceBuildingAt(BuildingType building, int x, int y)
        {
            var prefab = buildingLookup[building];
            var b = Instantiate(prefab, new Vector3(x, y), Quaternion.identity);
            b.center = Grid.GetTile(x, y);
            buildings.Add(b);
            return b;
        }
        
        [Button]
        public void EnableSwimming()
        {
            canSwimInSea.AddValue(1);
            foreach (var tile in _grid)
            {
                if (tile.Type == TileType.Ocean)
                {
                    tile.UpdatePathing();
                    if (tile.UpNeighbour()?.Type == TileType.Air) tile.UpNeighbour().UpdatePathing();
                }
            }
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

        public Building GetBuildingAt(Tile tile)
        {
            var tX = tile.X;
            var tY = tile.Y;
            foreach (var building in buildings)
            {
                var halfW = building.width / 2;
                var bL = building.center.X - halfW;
                var bR = building.center.X + halfW;
                var bT = building.center.Y + 1;
                var bB = building.center.Y - 1;

                if (tX >= bL && tX <= bR && tY >= bB && tY <= bT)
                {
                    return building;
                }
            }
            return null;
        }
    }
}
