using System;
using Game;
using Library;
using TMPro;
using UnityEngine.UI;

namespace Upgrades
{
    public class UpgradeInfo : MonoSingleton<UpgradeInfo>
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public TextMeshProUGUI cost;
        public Image resourceIcon;

        private void Start()
        {
            Hide();
        }

        public void Show(string name, string text, ResourceType resourceType, int resourceCost)
        {
            print($"Show {name}");
            title.text = name;
            description.text = text;
            if (resourceCost > 0)
            {
                resourceIcon.gameObject.SetActive(true);
                cost.gameObject.SetActive(true);
                resourceIcon.sprite = ResourcesController.Instance.resourceSprites[resourceType];
                cost.text = $"{resourceCost}";
            }
            else
            {
                resourceIcon.gameObject.SetActive(false);
                cost.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}