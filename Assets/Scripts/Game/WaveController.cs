using System;
using System.Collections.Generic;
using Framework;
using Library;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class WaveController : MonoSingleton<WaveController>
    {
        public FloatParameter waveSpawnRate;
        public FloatParameter waveSpawnSize;
        public FloatParameter waveMoveSpeed;

        public Wave wavePrefab;

        public List<Wave> waves; 
        public float spawnTimer;

        public static readonly Vector3 WaveStartPos = new(-4, 63.5f);
        public static readonly Vector3 WaveEndPos = new(49, 63.5f);

        protected override void Awake()
        {
            base.Awake();
            waves = new List<Wave>();
            spawnTimer = 1/waveSpawnRate;
        }

        private void Update()
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }

            if (spawnTimer <= 0)
            {
                var spawnTime = 1 / waveSpawnRate;
                spawnTimer = Random.Range(spawnTime * 0.8f, spawnTime * 1.2f);
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            var newWave = Instantiate(wavePrefab, WaveStartPos, Quaternion.identity, transform);
            newWave.transform.SetScale(waveSpawnSize);
            waves.Add(newWave);
        }

        public void RemoveWave(Wave wave)
        {
            if (waves.Contains(wave)) waves.Remove(wave);
        }
        
    }
}