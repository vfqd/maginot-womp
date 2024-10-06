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

        private void Start()
        {
            if (Random.value < plasticSpawnChance)
            {
                plastic = ResourcesController.Instance.CreateResourcePileAt(ResourceType.Plastic, plasticPos.position, 0,false);
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
        private void Die()
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