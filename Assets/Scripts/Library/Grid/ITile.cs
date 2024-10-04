using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.Grid
{
    public interface ITile
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Grid<T> GetGrid<T>() where T : ITile, new();
    }

    public static class ITileExtensions
    {
        public static List<T> Get4Neighbours<T>(this T tile) where T : ITile, new()
        {
            return tile.GetGrid<T>().Get4NeighboursEnumerable(tile).ToList();
        }
        
        public static IEnumerable<T> Get4NeighboursEnumerable<T>(this T tile) where T : ITile, new()
        {
            return tile.GetGrid<T>().Get4NeighboursEnumerable(tile);
        }
        
        public static List<T> Get8Neighbours<T>(this T tile) where T : ITile, new()
        {
            return tile.GetGrid<T>().Get8NeighboursEnumerable(tile).ToList();
        }
        
        public static IEnumerable<T> Get8NeighboursEnumerable<T>(this T tile) where T : ITile, new()
        {
            return tile.GetGrid<T>().Get8NeighboursEnumerable(tile);
        }
        
        public static T UpNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X, tile.Y + 1);
        public static T DownNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X, tile.Y - 1);
        public static T LeftNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X - 1, tile.Y);
        public static T RightNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X + 1, tile.Y);
        
        public static T UpRightNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X + 1, tile.Y + 1);
        public static T DownLeftNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X - 1, tile.Y - 1);
        public static T UpLeftNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X - 1, tile.Y + 1);
        public static T DownRightNeighbour<T>(this T tile) where T : ITile, new() => tile.GetGrid<T>().GetTile(tile.X + 1, tile.Y - 1);
        
        public static T GetNeighbourFromDelta<T>(this T tile, int dX, int dY) where T : ITile, new()
        {
            return tile.GetGrid<T>().GetTile(tile.X + dX, tile.Y + dY);
        }
        
        public static Vector2Int GetDeltaFromNeighbour<T>(this T tile, T neighbour) where T : ITile, new()
        {
            return new Vector2Int(neighbour.X - tile.X, neighbour.Y - tile.Y);
        }
        
        public static int Get4NeighbourCountWithCondition<T>(this T tile, Func<T,bool> condition) where T : ITile, new()
        {
            int count = 0;
            foreach (var neighbour in tile.Get4NeighboursEnumerable())
            {
                if (condition.Invoke(neighbour))
                {
                    count++;
                }
            }
            return count;
        }
        
        public static int Get8NeighbourCountWithCondition<T>(this T tile, Func<T,bool> condition) where T : ITile, new()
        {
            int count = 0;
            foreach (var neighbour in tile.Get8NeighboursEnumerable())
            {
                if (condition.Invoke(neighbour))
                {
                    count++;
                }
            }
            return count;
        }

        public static List<T> Get4NeighboursWithCondition<T>(this T tile, Func<T,bool> condition) where T : ITile, new()
        {
            List<T> neighbours = new List<T>();
            foreach (var neighbour in tile.Get4NeighboursEnumerable())
            {
                if (condition.Invoke(neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours;
        }

        public static List<T> Get8NeighboursWithCondition<T>(this T tile, Func<T,bool> condition) where T : ITile, new()
        {
            List<T> neighbours = new List<T>();
            foreach (var neighbour in tile.Get8NeighboursEnumerable())
            {
                if (condition.Invoke(neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours;
        }

        // We can't use ToString here as it gets overriden by the object's ToString
        public static string DebugString<T>(this T tile) where T : ITile
        {
            return $"{tile.GetType().Name} at [{tile.X},{tile.Y}]";
        }
    }
}