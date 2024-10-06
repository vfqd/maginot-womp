using System;
using TMPro;
using UnityEngine;

namespace Upgrades
{
    public class UnlockCircle : MonoBehaviour
    {
        public bool isUnlocked;
        public int numReq;
        public TextMeshProUGUI unlockText;

        private void Update()
        {
            var numLeft = numReq - UpgradeNode.totalResearchesDone;
            if (numLeft <= 0)
            {
                isUnlocked = true;
                gameObject.SetActive(false);
            }
            unlockText.text = $"{numLeft} more to reveal";
        }
    }
}