using System;
using System.Collections.Generic;
using Game;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeNode : SerializedMonoBehaviour,IPointerEnterHandler, IPointerExitHandler
    {
        [ShowInInspector,ReadOnly] public static int totalResearchesDone;
        
        public Image bg;
        public Image border;
        public Button button;

        public UnlockCircle countRequired;
        public int canBeBoughtTimes = 1;
        public int hasBeenBought = 0;
        
        public ResourceType resourceReq;
        public int baseResourceCost;

        public int ResourceCost => baseResourceCost * (hasBeenBought + 1);
        
        public TextMeshProUGUI nameText;

        public string name;
        [TextArea] public string description;

        public UpgradeEffect effect;
        public UpgradeEffect secondEffect;

        public List<UpgradeNode> toEnableWhenBought;

        private void Start()
        {
            UpdateName();
        }

        private void Update()
        {
            button.interactable = CanClick();   
            UpdateName();
        }

        private void UpdateName()
        {
            nameText.text = $"{name} {hasBeenBought}/{canBeBoughtTimes}";
        }

        public bool CanClick()
        {
            if (!gameObject.activeSelf) return false;
            if (hasBeenBought >= canBeBoughtTimes)
            {
                border.color = Color.yellow;
                foreach (var upgradeNode in toEnableWhenBought)
                {
                    upgradeNode.TryEnable();
                }
                return false;
            }
            var canAfford = ResourcesController.Instance.resourceCounts[resourceReq] >= ResourceCost;
            if (!canAfford)
            {
                border.color = Color.red;
                return false;
            }
            border.color = Color.green;
            return true;
        }

        private void TryEnable()
        {
            if (countRequired.isUnlocked)
            {
                gameObject.SetActive(true);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpgradeInfo.Instance.Show(nameText.text,description, resourceReq, ResourceCost);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UpgradeInfo.Instance.Hide();
        }

        public void ExecuteEffect()
        {
            ResourcesController.Instance.ChangeResourceValue(resourceReq,-ResourceCost);
            hasBeenBought++;
            if (hasBeenBought >= canBeBoughtTimes)
            {
                totalResearchesDone++;
            }
            UpdateName();
            UpgradeInfo.Instance.Show(nameText.text,description, resourceReq, ResourceCost);
            effect?.Execute(this);
            secondEffect?.Execute(this);
        }
    }
}