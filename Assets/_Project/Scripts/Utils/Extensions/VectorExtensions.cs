using UnityEngine;

namespace Project.Utils.Extensions
{
    public static class VectorExtensions 
    {
        public static Vector3 WithZeroY(this Vector3 vector) 
        {
            return new Vector3(vector.x, 0f, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
    }
}