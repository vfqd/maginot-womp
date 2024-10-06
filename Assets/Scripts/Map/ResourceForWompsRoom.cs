using Game;
using UnityEngine;

namespace Map
{
    public class ResourceForWompsRoom : MonoBehaviour, IWompSpawner
    {
        public ResourceType resourceType;
        
        public void AddNewWomp(GameObject wompPrefab)
        {
            ResourcesController.Instance.ChangeResourceValue(resourceType,1);
        }
    }
}