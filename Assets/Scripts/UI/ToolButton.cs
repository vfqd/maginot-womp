using System;
using Game;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToolButton : MonoBehaviour
    {
        public Tool tool;
        public BuildingType buildingType;

        public ResourceType resourceCost;
        public FloatParameter cost;

        public GameObject costObject;
        public TextMeshProUGUI costText;
        public Image costIcon;

        public Button button;
        public string tooltip;

        private bool _hasCost;
        public bool HasCost => _hasCost;

        private void Start()
        {
            if (cost == null || cost == 0)
            {
                costObject.SetActive(false);
                return;
            }

            _hasCost = true;
            costIcon.sprite = ResourcesController.Instance.resourceSprites[resourceCost];
            costText.text = $"{Mathf.RoundToInt(cost)}";
        }

        private void Update()
        {
            CheckCanAfford();
        }

        public bool CheckCanAfford()
        {
            if (!_hasCost) return true;
            costText.text = $"{Mathf.RoundToInt(cost)}";
            button.interactable = ResourcesController.Instance.resourceCounts[resourceCost] >= cost;
            return button.interactable;
        }
    }
}