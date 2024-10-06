using Library.UiAnimation;
using Map;
using TMPro;
using UnityEngine;

namespace UI
{
    public class BuildingInfoPanel : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI count;
        
        public void Show(Building building)
        {
            title.text = building.buildingName;
            count.text = $"{Mathf.RoundToInt(building.wompCount)}/{Mathf.RoundToInt(building.maxWompsAllowed)}";
            transform.position = building.transform.position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateCount(Building building)
        {
            count.text = $"{Mathf.RoundToInt(building.wompCount)}/{Mathf.RoundToInt(building.maxWompsAllowed)}";
        }
    }
}