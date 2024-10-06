using System;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class ShowUpgradesButton : MonoBehaviour
    {
        public GameObject circle;
        public UpgradePanel upgradePanel;

        private void Update()
        {
            bool enable = false;
            foreach (var upgradeNode in upgradePanel.AllUpgrades)
            {
                if (upgradeNode.CanClick())
                {
                    enable = true;
                    break;
                }
            }
            circle.SetActive(enable);
        }
    }
}