using System.Collections.Generic;
using Library.Extensions;
using UnityEngine;
using Womps;

namespace Map
{
    public class Building : MonoBehaviour
    {
        public GameObject wompPrefab;
        public BuildingType buildingType;
        public Tile center;
        public int width;
        
        public Tile GetRandomTileInBuilding()
        {
            List<Tile> options = new List<Tile>();
            var halfWidth = width / 2;
            for (int i = -halfWidth; i < halfWidth; i++)
            {
                var tile = MapController.Instance.Grid.GetTile(center.X + i, center.Y - 1);
                if (tile && tile.Type == TileType.Room) options.Add(tile);
            }
            return options.GetRandomElement();
        }

        public void AddWomp()
        {
            foreach (var wompSpawner in GetComponents<IWompSpawner>())
            {
                wompSpawner.AddNewWomp(wompPrefab);
            }
        }
    }

    public enum BuildingType
    {
        None,
        Hub,
        Diggers,
        Range,
        Bunks,
        Munitions,
        Lab,
        Surf
    }
}