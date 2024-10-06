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
            var womp = Instantiate(wompPrefab, _building.GetRandomTileInBuilding().transform.position,
                Quaternion.identity);
            if (womp.TryGetComponent<Womp>(out Womp w))
                w.home = _building;
        }
    }
}