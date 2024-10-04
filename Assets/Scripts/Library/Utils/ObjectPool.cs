using UnityEngine;

namespace Library.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;
        public int initialCount = 50;
        
        public void Start()
        {
            AddToPool(initialCount);
        }

        public GameObject GetPooledInstance(Vector3 position, Quaternion rotation, Transform newParent)
        {
            if (transform.childCount == 0)
            {
                AddToPool(10);
            }
            var instance = transform.GetChild(0);
            instance.SetParent(newParent);
            instance.SetPositionAndRotation(position,rotation);
            instance.gameObject.SetActive(true);
            return instance.gameObject;
        }
        
        public void ReturnPooledInstance(GameObject instance)
        {
            if (instance == null)
            {
                Debug.LogError("Instance null");
                return;
            }

            instance.SetActive(false);
            instance.transform.SetParent(transform);
        }

        private void AddToPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newObject = Instantiate(prefab, transform);
                newObject.SetActive(false);
            }
        }
    }
}