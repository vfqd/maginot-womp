using System;
using UI;

namespace Upgrades
{
    [Serializable]
    public class EnableButtonEffect : UpgradeEffect
    {
        public ToolButton button;
        
        public override void Execute(UpgradeNode node)
        {
            button.gameObject.SetActive(true);    
        }
    }
}