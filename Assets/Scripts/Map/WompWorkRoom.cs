using System;
using Game;
using UnityEngine;

namespace Map
{
    public class WompWorkRoom : MonoBehaviour, IWompSpawner
    {
        public FloatParameter timeToMakeResource;
        public FloatParameter workRatePerWomp;
        public float wompCount;
        public ResourceType resourceType;

        private float _timer;
        
        private Building _building;

        private void Awake()
        {
            _building = GetComponent<Building>();
        }

        private void Start()
        {
            _timer = timeToMakeResource;
        }

        public void AddNewWomp(GameObject wompPrefab)
        {
            wompCount++;
        }
        
        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime * workRatePerWomp * wompCount;
            }

            if (_timer <= 0)
            {
                _timer = timeToMakeResource;
                ResourcesController.Instance.CreateResourcePileAt(resourceType,_building.GetRandomTileInBuilding().transform.position,1,true);
            }
        }
    }
}