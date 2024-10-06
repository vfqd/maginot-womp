using System;
using DG.Tweening;
using Game;
using Library.Sprites;
using UnityEngine;

namespace Map
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private ParticleSystem hitEffect;

        public float projSpeed = 50;
        
        public FloatParameter reloadSpeed;
        public FloatParameter projectileDamage;
        public FloatParameter maxRange;

        public SpriteAnimation shootAnim;
        public SpriteAnimator animator;
        
        public bool targetClose;

        private float _reloadTimer;

        private void Start()
        {
            _reloadTimer = 1 / reloadSpeed;
        }

        private void Update()
        {
            if (_reloadTimer > 0)
            {
                _reloadTimer -= Time.deltaTime;
            }

            if (_reloadTimer <= 0)
            {
                _reloadTimer = 1 / reloadSpeed;
                ShootProj();
            }
        }

        private void ShootProj()
        {
            var target = targetClose
                    ? WaveController.Instance.GetNearestWaveInRange(transform.position, maxRange)
                    : WaveController.Instance.GetFurthestWaveInRange(transform.position, maxRange);
            
            print($"Try shoot {target}");

            if (target && !target.willDie)
            {
                target.willDie = true;
                animator.Play(shootAnim,reloadSpeed, () =>
                {
                    if (!target) return;
                    target.willDie = true;
                    var proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
                    var d = Mathf.Abs(transform.position.x - target.transform.position.x);
                
                    var start = shootPoint.position;
                    var end = target.transform.position;
                    var mid = new Vector3((start.x + end.x) / 2, start.y + 4);
                    proj.transform.DOPath(new[] { start, mid, end }, d / projSpeed, PathType.CatmullRom)
                        .OnComplete(() =>
                        {
                            if (target) target.Die();
                            if (hitEffect) Destroy(Instantiate(hitEffect, proj.transform.position, Quaternion.identity).gameObject,3);
                            Destroy(proj.gameObject);
                            target = null;
                        });
                });
            }
        }
    }
}