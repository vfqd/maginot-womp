using System;
using DG.Tweening;
using Framework;
using Game;
using Library.Grid;
using Library.Sprites;
using Map;
using Pathfinding;
using UnityEngine;

namespace Womps
{
    public class Runner : MonoBehaviour
    {
        private Womp _womp;
        public ResourcePile targetPile;

        public Sprite headband;
        public Sprite diveMask;

        public RunState runState;
        
        private float _checkTimer = 1;

        private float _stillTimer = 0.1f;
        private Vector3 _prevPos;
        
        public enum RunState
        {
            Idle,
            GoingToPile,
            GoingHome
        }
        
        private void Awake()
        {
            _womp = GetComponent<Womp>();
        }

        public void Update()
        {
            if (_checkTimer > 0)
            {
                _checkTimer -= Time.deltaTime;
            }

            if (_checkTimer <= 0)
            {
                _checkTimer = 0.5f;

                switch (runState)
                {
                    case RunState.Idle:
                        if (targetPile == null)
                        {
                            TryGetNewTarget();
                        }
                        break;
                    case RunState.GoingToPile:
                        UpdatePathToTile();
                        break;
                    case RunState.GoingHome:
                        // Hack
                        var d = Vector3.Distance(transform.position, _prevPos);
                        if (d < 0.01f)
                        {
                            _stillTimer -= Time.deltaTime;
                            if (_stillTimer <= 0) runState = RunState.GoingToPile;
                        }
                        else
                        {
                            _stillTimer = 0.1f;
                        }

                        if (targetPile)
                        {
                            targetPile.transform.localPosition = new Vector3(0, 0.8f);
                        }
                        break;
                }
            }

            _womp.spriteAnimator.hat.sprite = _womp.currentTile.Type == TileType.Ocean ? diveMask : headband;
            _prevPos = transform.position;
        }

        private void TryGetNewTarget()
        {
            targetPile = ResourcesController.Instance.GetHighestValueResourcePile(transform.position);
            if (targetPile == null)
            {
                _womp.SetDestination(_womp.home.GetRandomTileInBuilding().transform.position,null);
                return;
            }
            
            ResourcesController.Instance.ReserveResourcePile(this,targetPile);
            runState = RunState.GoingToPile;
        }

        private void UpdatePathToTile()
        {
            _womp.SetDestination(targetPile.transform.position, null);
            if (Vector3.Distance(targetPile.transform.position, transform.position) < 1.6f)
            {
                runState = RunState.GoingHome;
                targetPile.pickedUp = true;
                targetPile.DOKill();
                targetPile.transform.parent = transform;
                targetPile.enabled = false;
                targetPile.transform.SetLocalX(0);
                targetPile.transform.SetLocalY(0.8f);
                targetPile.GetComponent<SpriteRenderer>().sortingOrder = 2;
                UpdatePathHome();
            }
        }

        private void UpdatePathHome()
        {
            _womp.SetDestination(ResourcesController.Instance.GetHubDropPoint(), () =>
            {
                ResourcesController.Instance.DepositResourcePile(this,targetPile);
                targetPile = null;
                runState = RunState.Idle;
            });
        }
    }
}