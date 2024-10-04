using System;
using Framework;
using UnityEngine;

namespace Library.Sprites
{
    public class GetZValueFromY : MonoBehaviour
    {
        public bool changeOnUpdate;

        [SerializeField] private float offset;

        private void Awake()
        {
            UpdateZPos();
        }

        public void Update()
        {
            if (!changeOnUpdate) return;
            UpdateZPos();
        }

        public void LateUpdate()
        {
            if (!changeOnUpdate) return;
        }

        public void UpdateZPos()
        {
            transform.SetZ(transform.position.y / 100 + offset);
        }
    }
}