using System;
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

        public RunState runState;
        
        private float _checkTimer = 1;
        
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
                }
                
            }
        }

        private void TryGetNewTarget()
        {
            targetPile = ResourcesController.Instance.GetNearestResourcePileTo(transform.position);
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
            if (Vector3.Distance(targetPile.transform.position, transform.position) < 0.01f)
            {
                runState = RunState.GoingHome;
                targetPile.transform.parent = transform;
                targetPile.enabled = false;
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