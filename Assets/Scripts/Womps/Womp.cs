using System;
using Library.Grid;
using Library.Sprites;
using Map;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace Womps
{
    public class Womp : MonoBehaviour
    {
        public FloatParameter moveSpeedMultiplier;
        public float baseMoveSpeed;
        public float baseJumpSpeed;
        public float baseLadderSpeed;
        
        public SpriteAnimation idleAnim;
        public SpriteAnimation runAnim;
        public SpriteAnimation climbAnim;
        public SpriteAnimation jumpAnim;
        
        public SpriteRenderer spriteRenderer;
        public SpriteAnimator spriteAnimator;
        public Tile currentTile;

        public bool visualsLocked;
        public Building home;
        
        private AILerp _path;
        private Vector3 _prevPos;

        private float _moveSpeed;
        private float _jumpSpeed;
        private float _ladderSpeed;
        private Action _arrivedCallback;

        private void Awake()
        {
            _path = GetComponent<AILerp>();
            _path.speed = baseMoveSpeed;
            _moveSpeed = baseMoveSpeed;
            _jumpSpeed = baseJumpSpeed;
            _ladderSpeed = baseLadderSpeed;
        }

        private void Start()
        {
            // _path.destination = new Vector3(55,64);
            // spriteRenderer.flipX = pos.x < transform.position.x;    
        }

        private void Update()
        {
            // _path.destination = destination.position;
            
            var position = transform.position;
            var moveDelta = (position) - _prevPos;
            
            var x = position.x;
            var y = position.y;
            currentTile = MapController.Instance.Grid.GetTile(x, y);
            
            _moveSpeed = baseMoveSpeed * moveSpeedMultiplier;
            _jumpSpeed = baseJumpSpeed * moveSpeedMultiplier;
            _ladderSpeed = baseLadderSpeed * moveSpeedMultiplier;

            if (!visualsLocked)
            {
                if (moveDelta.magnitude <= 0.001f && !currentTile.CanBeStoodOn())
                {
                    SetDestination(currentTile.GetNeighbourThatCanBeStoodOn(),null);
                }
                
                spriteRenderer.flipX = moveDelta.x > 0;
                UpdateVisuals(position,moveDelta);
            }
            _prevPos = transform.position;
        }

        public void SetDestination(Tile tile, Action callbackWhenArrived)
        {
            var target = tile;
            if (tile == null) return;
            if (!tile.CanBeStoodOn())
            {
                target = tile.GetNeighbourThatCanBeStoodOn();
            }
            if (target == null) return;
            
            _path.enabled = true;
            _path.destination = target.transform.position;
            _path.SearchPath();
            _arrivedCallback = callbackWhenArrived;
        }
        
        public void SetDestination(Vector3 position, Action callbackWhenArrived)
        {
            _path.enabled = true;
            _path.destination = position;
            _path.SearchPath();
            _arrivedCallback = callbackWhenArrived;
        }

        /*public void ClearPath()
        {
            _path.
        }*/

        private void UpdateVisuals(Vector3 position, Vector3 moveDelta)
        {
            var distToDest = Vector3.Distance(position, _path.destination);

            if (_path.reachedEndOfPath || !_path.enabled)
            {
                _path.speed = 1;
                PlayIfNotPlaying(idleAnim, 1);
                if (_arrivedCallback != null)
                {
                    _arrivedCallback.Invoke();
                    _arrivedCallback = null;
                }
                // _path.enabled = false;
            }
            else if (currentTile && currentTile.Type == TileType.Ladder)
            {
                _path.speed = _ladderSpeed;
                PlayIfNotPlaying(climbAnim,_ladderSpeed/baseLadderSpeed);
            }
            else if (moveDelta.y > 0)
            {
                _path.speed = _jumpSpeed;
                PlayIfNotPlaying(jumpAnim,_jumpSpeed/baseJumpSpeed);
            }
            else
            {
                _path.speed = _moveSpeed;
                PlayIfNotPlaying(runAnim,_moveSpeed/baseMoveSpeed);
            }
        }

        private void PlayIfNotPlaying(SpriteAnimation spriteAnimation, float multiplier)
        {
            if (spriteAnimator.ActiveAnimation == spriteAnimation) return;
            spriteAnimator.Play(spriteAnimation,multiplier);
        }
    }
}