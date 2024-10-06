using System;
using Framework;
using UI;

namespace Upgrades
{
    [Serializable]
    public class AddToFloatParameterEffect : UpgradeEffect
    {
        public string paramName;
        public float amount;
        
        public override void Execute(UpgradeNode node)
        {
            var param = ScriptableEnum.GetValue<FloatParameter>(paramName);
            param.AddValue(amount);   
        }
    }
}