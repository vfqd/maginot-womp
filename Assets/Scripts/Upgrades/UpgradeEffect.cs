using System;

namespace Upgrades
{
    [Serializable]
    public abstract class UpgradeEffect
    {
        public abstract void Execute(UpgradeNode node);
    }
}