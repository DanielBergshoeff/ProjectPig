using System.Linq;
using UnityEngine;

public static class Vector3Extension
{

    public static float LongestAxis(this Vector3 vector)
    {
        float[] axis = new float[] { vector.x, vector.y, vector.z };
        return axis.Max();
    }
}
