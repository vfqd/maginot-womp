using System;
using Library.Extensions;
using UnityEngine;

namespace Game
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 prevPos;

        private void Update()
        {
            var dir = (transform.position - prevPos).normalized;
            transform.rotation = transform.rotation.RotateToFace2D(dir, 360);
            prevPos = transform.position;
        }
    }
}