using System;
using Map;
using UI;

namespace Upgrades
{
    [Serializable]
    public class EnableSwimmingEffect : UpgradeEffect
    {
        public override void Execute(UpgradeNode node)
        {
            MapController.Instance.EnableSwimming();  
        }
    }
}