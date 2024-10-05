using System;
using UnityEngine;
using Womps;

namespace Map
{
    public class BasicWompHome : MonoBehaviour, IWompSpawner
    {
        private Building _building;

        private void Awake()
        {
            _building = GetComponent<Building>();
        }

        public void AddNewWomp(GameObject wompPrefab)
        {
            var miner = Instantiate(wompPrefab, _building.GetRandomTileInBuilding().transform.position,
                Quaternion.identity);
            miner.GetComponent<Womp>().home = _building;
        }
    }
}