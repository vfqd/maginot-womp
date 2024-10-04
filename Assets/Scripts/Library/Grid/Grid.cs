using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Library.Grid
{
    public class Grid<T> : IEnumerable<T> where T : ITile, new()
    {       
        public T this[int x, int y]
        {
            get => GetTile(x, y);
            set {
                if (ValidCoords(x, y))
                {
                    _map[Idx(x, y)] = value;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public int Width => _width;
        public int Height => _height;
        
        private readonly T[] _map;
        private readonly int _width;
        private readonly int _height;

        private int Idx(int x, int y) => y * _width + x;
        
        // Use as pure data
        public Grid(int width, int height, Action<T,int,int> initializationAction = null)
        {
            _map = new T[width * height];
            _width = width;
            _height = height;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _map[Idx(x,y)] = new T
                    {
                        X = x,
                        Y = y
                    };
                    initializationAction?.Invoke(_map[Idx(x,y)],x,y);
                }    
            }
        }

        // Use with MonoBehaviours
        public Grid(int width, int height, MonoBehaviour prefab, Transform tileParent = null, Action<T,int,int> initializationAction = null)
        {
            _map = new T[width * height];
            _width = width;
            _height = height;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    _map[Idx(x,y)] = Object.Instantiate(prefab, new Vector3(x,y), Quaternion.identity,tileParent).GetComponent<T>();
                    _map[Idx(x,y)].X = x;
                    _map[Idx(x,y)].Y = y;
                    initializationAction?.Invoke(_map[Idx(x,y)],x,y);
                }
            }
        }

        public T GetTile(int x, int y)
        {
            if (!ValidCoords(x,y)) return default;
            return _map[Idx(x,y)];
        }

        public bool GetTile(int x, int y, out T tile)
        {
            tile = GetTile(x, y);
            return tile != null;
        }

        public T GetTile(float x, float y)
        {
            return GetTile(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }
        
        public bool GetTile(float x, float y, out T tile)
        {
            tile = GetTile(x, y);
            return tile != null;
        }

        public bool ValidCoords(int x, int y) => x >= 0 && x < _width && y >= 0 && y < _height;

        public IEnumerable<T> Get4NeighboursEnumerable(T t)
        {
            if (GetTile(t.X, t.Y + 1, out var u)) yield return u;
            if (GetTile(t.X, t.Y - 1, out var d)) yield return d;
            if (GetTile(t.X - 1, t.Y, out var l)) yield return l;
            if (GetTile(t.X + 1, t.Y, out var r)) yield return r;
        }
        
        public IEnumerable<T> Get8NeighboursEnumerable(T t)
        {
            if (GetTile(t.X, t.Y + 1, out var u)) yield return u;
            if (GetTile(t.X, t.Y - 1, out var d)) yield return d;
            if (GetTile(t.X - 1, t.Y, out var l)) yield return l;
            if (GetTile(t.X + 1, t.Y, out var r)) yield return r;
            
            if (GetTile(t.X - 1, t.Y + 1, out var ul)) yield return ul;
            if (GetTile(t.X + 1, t.Y + 1, out var ur)) yield return ur;
            if (GetTile(t.X - 1, t.Y - 1, out var dl)) yield return dl;
            if (GetTile(t.X + 1, t.Y - 1, out var dr)) yield return dr;
        }

        public void ForEachXY(Action<int, int> callback)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    callback.Invoke(x,y);
                }
            }
        }

        public bool IsEdgeTile(T t)
        {
            return (t.X == 0 || t.X == _width - 1 || t.Y == 0 || t.Y == _height);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _map.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}