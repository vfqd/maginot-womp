using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Library;
using Library.Grid;
using Map;
using Pathfinding;
using Sirenix.Serialization;
using UnityEngine;

namespace Game
{
    public class MiningController : SerializedMonoSingleton<MiningController>
    {
        [OdinSerialize] private HashSet<Tile> targetsToMine;
        public GameObject mineTargetAnim;

        private Dictionary<Tile, GameObject> _targetAnims;

        protected override void Awake()
        {
            base.Awake();
            targetsToMine = new HashSet<Tile>();
            _targetAnims = new Dictionary<Tile, GameObject>();
        }

        public void AddTileToMiningList(Tile tile)
        {
            if (targetsToMine.Contains(tile)) return;
            // if (tile.DownNeighbour()?.Type == TileType.Room) return;
            // if (tile.UpNeighbour()?.Type == TileType.Room) return;
            if (tile.Get8NeighbourCountWithCondition((neighbour => neighbour.Type == TileType.Ocean))>0) return;
            
            targetsToMine.Add(tile);
            _targetAnims[tile] = Instantiate(mineTargetAnim, tile.transform.position.WithZ(-0.1f), Quaternion.identity, transform);
        }

        public void RemoveTileFromMiningList(Tile tile)
        {
            if (!targetsToMine.Contains(tile)) return;
            targetsToMine.Remove(tile);
            Destroy(_targetAnims[tile].gameObject);
        }

        public Tile GetBestTileToMine(Vector3 position)
        {
            List<Tile> options = new List<Tile>();
            NNInfo posNode = AstarPath.active.GetNearest(position);
            foreach (var tile in targetsToMine)
            {
                if (tile.Get8NeighbourCountWithCondition((neighbour =>
                    {
                        if (!neighbour.CanBeStoodOn()) return false;
                        NNInfo nNode = AstarPath.active.GetNearest(neighbour.transform.position);
                        return PathUtilities.IsPathPossible(posNode.node,nNode.node);
                    })) > 0)
                {
                    options.Add(tile);
                }
            }
            if (options.Count > 0)
            {
                var ordered = options
                    .OrderBy(t =>
                    {
                        Vector3 tilePosition = t.transform.position;
                        // Whatever end of path is closest to its destination
                        return ABPath.Construct(position, tilePosition).vectorPath.Count;
                    })
                    .ThenByDescending(t => t.Y);
                
                return ordered.First();
            }
            
            return null;
        }
    }
}