using UnityEngine;
using Random = System.Random;

namespace Library.Extensions
{
    public static class SystemRandomExtensions
    {
        public static float Range(this Random random, float a, float b)
        {
            return (float) (random.NextDouble() * (b-a) + a);
        }
        
        public static Vector2 InsideUnitCircle(this Random random)
        {
            float a = random.Next() * 2 * Mathf.PI;
            float r = Mathf.Sqrt(random.Next());
            float x = r * Mathf.Cos(a);
            float y = r * Mathf.Sin(a);
            return new Vector2(x, y);
        }
    }
}