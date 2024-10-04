using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.Grid.SampleUsage
{
    public class SampleTile : MonoBehaviour, ITile
    {
        public SpriteRenderer spriteRenderer;
        
        private Grid<SampleTile> _grid;

        public int X { get; set; }
        public int Y { get; set; }
        public Grid<T> GetGrid<T>() where T : ITile, new()
        {
            _grid ??= SampleMap.Instance.Grid;
            return _grid as Grid<T>;
        }
    }
}