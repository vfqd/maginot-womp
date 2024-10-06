using System;
using System.Linq;
using Library;
using Map;
using Sirenix.OdinInspector;
using UnityEngine;
using Womps;

namespace Game
{
    public class GameController : MonoSingleton<GameController>
    {
        public FloatParameter ladderCost;
        public FloatParameter castleCost;
        
        public float wompsAvailable;
        
        protected override void Awake()
        {
            base.Awake();
            foreach (var floatParameter in FloatParameter.AllFloatParameters)
            {
                floatParameter.ResetValue();
            }
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            var hub = MapController.Instance.PlaceBuildingAt(BuildingType.Hub,67,56);
            ResourcesController.Instance.hub = hub;
            hub.AddWomp();
            var digs = MapController.Instance.PlaceBuildingAt(BuildingType.Diggers,57,56);
            digs.AddWomp();
        }

        private void Update()
        {
            var currentWomps = MapController.Instance.buildings.Sum((building => building.wompCount)) - 2; // 2 free womps at start
            var wompsAllowed =
                MapController.Instance.buildings.Count(building => building.buildingType == BuildingType.Bunks) *
                MapController.Instance.buildingLookup[BuildingType.Bunks].maxWompsAllowed;
            wompsAvailable = wompsAllowed - currentWomps;
            ResourcesController.Instance.SetResourceValue(ResourceType.Womps,wompsAvailable);
        }

        public bool CanAffordWomps()
        {
            return wompsAvailable > 0;
        }
    }
}