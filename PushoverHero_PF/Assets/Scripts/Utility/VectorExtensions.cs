using UnityEngine;

namespace Utility
{
    public static class VectorExtensions
    {
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad; 

            float x = vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians);
            float y = vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians);

            return new Vector2(x, y);
        }

        public static Vector3 RotateY(this Vector3 vector, float degrees)
        {
            var rotation = Quaternion.Euler(0, degrees, 0);

            return rotation * vector;
        }
    }
}
