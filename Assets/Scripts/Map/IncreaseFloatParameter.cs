using System;
using UnityEngine;

namespace Map
{
    public class IncreaseFloatParameter : MonoBehaviour
    {
        public FloatParameter toIncrease;
        public FloatParameter byAmount;
        
        private void Start()
        {
            toIncrease.AddValue(byAmount);    
        }
    }
}