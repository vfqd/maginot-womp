using Library;

namespace Upgrades
{
    public class UpgradePanel : MonoSingleton<UpgradePanel>
    {
        public UpgradeNode[] AllUpgrades;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}