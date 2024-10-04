using UnityEngine;

namespace Library.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion RotateToFace2D(this Quaternion startingRot, Vector2 dir, float maxDegreesDelta)
        {
            float zRot = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg - 90;
            var rot = Quaternion.Euler(0, 0, zRot);
            var rotateTowards = Quaternion.RotateTowards(startingRot, rot, maxDegreesDelta);
            return rotateTowards;
        }
    }
}