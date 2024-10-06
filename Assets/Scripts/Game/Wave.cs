using System;
using Framework;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using VFavorites.Libs;
using Random = UnityEngine.Random;

namespace Game
{
    public class Wave : MonoBehaviour
    {
        public Transform plasticPos;
        public ResourcePile plastic;
        
        public FloatParameter plasticSpawnChance;
        public FloatParameter plasticMaxValue;
        public FloatParameter waveMoveSpeed;
        public FloatParameter waveSize;
        public FloatParameter canSpawnSand;

        public bool willDie;

        private void Start()
        {
            var plasticSpawn = Mathf.Min(plasticSpawnChance, 0.5f);
            if (Random.value < plasticSpawn)
            {
                var resourceType = Random.value < 0.33f ? ResourceType.Metal : ResourceType.Plastic;
                plastic = ResourcesController.Instance.CreateResourcePileAt(resourceType, plasticPos.position, 0,false);
            }
            else
            {
                if (canSpawnSand > 0.5f)
                {
                    if (Random.value < 0.333f)
                    {
                        plastic = ResourcesController.Instance.CreateResourcePileAt(ResourceType.Sand, plasticPos.position, 0,false);
                    }
                }
            }
        }

        private void Update()
        {
            transform.position += new Vector3(waveMoveSpeed * Time.deltaTime, 0);
            var startX = WaveController.WaveStartPos.x;
            var endX = WaveController.WaveEndPos.x;
            var t = (transform.position.x - startX) / (endX - startX);
            transform.SetScale(Mathf.Lerp(waveSize,0.25f,t));

            if (plastic)
            {
                plastic.transform.position = plasticPos.position;
                plastic.value = Mathf.Max(1,Mathf.Lerp(plasticMaxValue, 0, t));
            }
            
            if (transform.position.x > endX)
            {
                Die();
            }
        }

        [Button]
        public void Die()
        {
            if (plastic)
            {
                plastic.canBeCollected = true;
            }
            WaveController.Instance.RemoveWave(this);
            Destroy(gameObject);
        }
    }
}