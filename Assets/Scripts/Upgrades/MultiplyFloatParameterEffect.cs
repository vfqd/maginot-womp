using System;
using Framework;
using Sirenix.OdinInspector;
using UI;

namespace Upgrades
{
    [Serializable]
    public class MultiplyFloatParameterEffect : UpgradeEffect
    {
        public string paramName;
        [InfoBox("10% increase = 0.1")]
        public float amount;
        
        public override void Execute(UpgradeNode node)
        {
            var param = ScriptableEnum.GetValue<FloatParameter>(paramName);
            param.MultiplyValue(1+amount);   
        }
    }
}