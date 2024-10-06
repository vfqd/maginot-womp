using System;
using System.Collections.Generic;
using Game;
using Library;
using UnityEngine;

namespace UI
{
    public class UiController : MonoSingleton<UiController>
    {
        public GameObject resourcesPanel;
        public Transform resourceRowParent;
        public ResourceRow resourceRowPrefab;
        public GameObject endgameScreen;

        public BuildingInfoPanel infoPanel;

        private Dictionary<ResourceType, ResourceRow> _rows;

        protected override void Awake()
        {
            base.Awake();
            _rows = new Dictionary<ResourceType, ResourceRow>();
        }

        private void Start()
        {
            resourcesPanel.SetActive(false);
        }

        private void Update()
        {
            foreach (var kvp in ResourcesController.Instance.resourceCounts)
            {
                if (!_rows.ContainsKey(kvp.Key))
                {
                    if (kvp.Value > 0)
                    {
                        resourcesPanel.SetActive(true);
                        var newRow = Instantiate(resourceRowPrefab, resourceRowParent);
                        newRow.icon.sprite = ResourcesController.Instance.resourceSprites[kvp.Key];
                        newRow.count.text = $"{kvp.Value:000000}";
                        _rows[kvp.Key] = newRow;
                    }
                }
                else
                {
                    _rows[kvp.Key].count.text = $"{kvp.Value:000000}";
                }
            }
        }

        public void PressEndgameContinue()
        {
            endgameScreen.SetActive(false);
        }
    }
}