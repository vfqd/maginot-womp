using System;
using System.Linq;
using Map;
using UnityEngine;

namespace Game
{
    public class Cheats : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    foreach (var resource in ResourcesController.Instance.resourceCounts.Keys.ToArray())
                    {
                        ResourcesController.Instance.ChangeResourceValue(resource,1000);
                    }
                }
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    foreach (var building in MapController.Instance.buildings)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            building.AddWomp();
                        }
                    }
                }
            }
        }
    }
}