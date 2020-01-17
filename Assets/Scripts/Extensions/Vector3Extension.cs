using System.Linq;
using UnityEngine;

public static class Vector3Extension
{
    public static float LongestAxis(this Vector3 vector)
    {
        float[] axis = new float[] { vector.x, vector.y, vector.z };
        return axis.Max();
    }

    public static Vector3 RandomVector(this Vector3 vector, float min = 0f, float max = 1f)
    {
        vector.x = Random.Range(min, max);
        vector.y = Random.Range(min, max);
        vector.z = Random.Range(min, max);
        return vector;
    }
}
