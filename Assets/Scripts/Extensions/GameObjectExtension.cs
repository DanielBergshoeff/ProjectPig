using UnityEngine;

public static class GameObjectExtension
{

    public static Vector3 GetSize(this GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;

        foreach (Renderer child in renderers)
        {
            bounds.Encapsulate(child.bounds);
        }

        return bounds.size;
    }
}
