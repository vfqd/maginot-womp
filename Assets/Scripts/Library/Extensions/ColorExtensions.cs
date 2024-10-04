using UnityEngine;

namespace Library.Extensions
{
    public static class ColorExtensions
    {
        public static bool IsEqualTo(this Color color, Color other)
        {
            // return Mathf.Approximately(color.r, other.r) &&
            //        Mathf.Approximately(color.g, other.g) &&
            //        Mathf.Approximately(color.b, other.b) &&
            //        Mathf.Approximately(color.a, other.a);
            return Mathf.Abs(color.r - other.r) < 0.01f &&
                   Mathf.Abs(color.g - other.g) < 0.01f &&
                   Mathf.Abs(color.b - other.b) < 0.01f &&
                   Mathf.Abs(color.a - other.a) < 0.01f;
        }
    }
}