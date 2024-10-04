using UnityEngine;

namespace Library.Grid.SampleUsage
{
    public class SampleMap : MonoSingleton<SampleMap>
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private SampleTile tilePrefab;
        
        public Grid<SampleTile> Grid => _grid;
        private Grid<SampleTile> _grid;

        protected override void Awake()
        {
            base.Awake();
            _grid = new Grid<SampleTile>(width, height, tilePrefab, transform);

            foreach (var sampleTile in _grid[1,1].Get4Neighbours())
            {
                print(sampleTile.spriteRenderer.sprite);
            }
        }
    }
}