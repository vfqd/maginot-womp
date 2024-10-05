using System;
using Game;
using Library.Grid;
using Library.Sprites;
using Map;
using Pathfinding;
using UnityEngine;

namespace Womps
{
    public class Miner : MonoBehaviour
    {
        private Womp _womp;
        public Tile targetTile;
        public Tile miningTile;

        public SpriteAnimation miningAnim;

        public FloatParameter miningDamage;
        public FloatParameter miningReloadTime;

        private float _attackTimer;
        private float _retaskTimer = 1;

        private void Awake()
        {
            _womp = GetComponent<Womp>();
        }

        public void Update()
        {
            // Need to mine our currentTarget
            if (miningTile)
            {
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0)
                {
                    _attackTimer = miningReloadTime;
                    miningTile.tileHealth.DealDamage(miningDamage);
                    if (miningTile.Type == TileType.Air)
                    {
                        miningTile = null;
                    }
                }
                return;
            }

            _womp.visualsLocked = false;
            
            // Need to get a new target
            _retaskTimer -= Time.deltaTime;
            if (_retaskTimer < 0)
            {
                _retaskTimer = 1;
                TryGetNewTarget();
            }
        }

        private void TryGetNewTarget()
        {
            targetTile = MiningController.Instance.GetBestTileToMine(transform.position);

            if (targetTile)
            {
                var toStandOn = GetBestTileToStandOn(targetTile);
                _womp.SetDestination(toStandOn, () =>
                {
                    // we will sometimes not get exactly to this tile
                    var dToTarget = Vector3.Distance(transform.position, targetTile.transform.position);
                    if (dToTarget < 3f)
                    {
                        _womp.visualsLocked = true;
                        miningTile = targetTile;
                        _womp.spriteAnimator.Play(miningAnim, miningReloadTime);
                        _womp.spriteRenderer.flipX = miningTile.transform.position.x > transform.position.x;
                    }
                    targetTile = null;
                });
            }
            else
            {
                targetTile = _womp.home.GetRandomTileInBuilding();
                _womp.SetDestination(targetTile,null);
            }
        }

        private Tile GetBestTileToStandOn(Tile target)
        {
            var dl = target.DownLeftNeighbour(); if (CheckTile(target, dl)) return dl;
            var dr = target.DownRightNeighbour(); if (CheckTile(target, dr)) return dr;
            var ul = target.UpLeftNeighbour(); if (CheckTile(target, ul)) return ul;
            var ur = target.UpRightNeighbour(); if (CheckTile(target, ur)) return ur;
            
            var l = target.LeftNeighbour(); if (CheckTile(target, l)) return l;
            var r = target.RightNeighbour(); if (CheckTile(target, r)) return r;
            var u = target.UpNeighbour(); if (CheckTile(target, u)) return u;
            var d = target.DownNeighbour(); if (CheckTile(target, d)) return d;

            var dd = target.DownNeighbour().DownNeighbour(); if (CheckTile(target, dd)) return dd;
            var ddl = target.DownNeighbour().DownLeftNeighbour(); if (CheckTile(target, ddl)) return ddl;
            var ddr = target.DownNeighbour().DownRightNeighbour(); if (CheckTile(target, ddr)) return ddr;
            
            return target;
        }

        private bool CheckTile(Tile target, Tile toCheck)
        {
            NNInfo ourPos = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
            if (toCheck && IsTypeStandable(toCheck.Type))
            {
                var down = toCheck.DownNeighbour();
                if (down && !IsTypeSolid(down.Type)) return false;
                
                NNInfo dlPos = AstarPath.active.GetNearest(toCheck.transform.position, NNConstraint.Default);
                if (PathUtilities.IsPathPossible(ourPos.node, dlPos.node))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTypeStandable(TileType type)
        {
            return type.IsNotSolid();
        }
        
        private bool IsTypeSolid(TileType type)
        {
            return type.IsSolid();
        }
    }
}