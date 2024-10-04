using UnityEngine;

namespace Library.Utils
{
    public class Converter
    {
        public const int PixelsPerWorldUnit = 8;

        public static Vector2 ConvertToIso(Vector2Int coords)
        {
            float isoX = coords.x + coords.y;
            float isoY = (coords.y - coords.x) / 2f;
            return new Vector2(isoX,isoY);
        }

        public static Vector2 ConvertToIso(int x, int y)
        {
            return ConvertToIso(new Vector2Int(x, y));
        }

        public static float SnapToWorldUnits(float input)
        {
            return Mathf.Round(input * PixelsPerWorldUnit) / PixelsPerWorldUnit;
        }

        public static float FloorToWorldUnits(float input)
        {
            return Mathf.Floor(input * PixelsPerWorldUnit) / PixelsPerWorldUnit;
        }

        public static float CeilToWorldUnits(float input)
        {
            return Mathf.Ceil(input * PixelsPerWorldUnit) / PixelsPerWorldUnit;
        }

        public static Vector3 SnapToWorldUnits(Vector3 input)
        {
            return new Vector3(SnapToWorldUnits(input.x), SnapToWorldUnits(input.y), SnapToWorldUnits(input.z));
        }

        public static Vector3 FloorToWorldUnits(Vector3 input)
        {
            return new Vector3(FloorToWorldUnits(input.x), FloorToWorldUnits(input.y), FloorToWorldUnits(input.z));
        }

        public static Vector3 CeilToWorldUnits(Vector3 input)
        {
            return new Vector3(CeilToWorldUnits(input.x), CeilToWorldUnits(input.y), CeilToWorldUnits(input.z));
        }
    }
}