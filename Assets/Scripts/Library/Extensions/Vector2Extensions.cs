using UnityEngine;

namespace Library.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 RotateByAngle(this Vector2 v, float a)
        {
            var rad = Mathf.Deg2Rad * a;
            var c = Mathf.Cos(rad);
            var s = Mathf.Sin(rad);
            return new Vector2(v.x * c + v.y * -s, v.x * s + v.y * c);  
        }
    }
}