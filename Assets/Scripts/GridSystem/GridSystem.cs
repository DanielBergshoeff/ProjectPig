using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour
{
    public GameObject gridObject;
    public Vector3 offset;
    private Vector3 Offset;
    private Vector3 gridOrigin;

    private List<GameObject> currentGrid = new List<GameObject>();
    private const string gridRootName = "GridRoot";

    private void Awake()
    {
        foreach (Transform child in transform.GetChild(0).transform)
            if (child.name == gridRootName)
                currentGrid.Add(child.gameObject);
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            gridObject.transform.localScale = Vector3.one;
            Vector3 objectSize = gridObject.GetSize();
            gridOrigin = new Vector3(transform.localScale.x * (objectSize.x + offset.x), transform.localScale.y, transform.localScale.z * (objectSize.z + offset.z));

            transform.hasChanged = false;

            ClearGrid(currentGrid);

            Vector2 point = transform.position;

            if (!gridObject)
            {
                print("Please give a tile object");
                return;
            }

            CreateGrid(gridObject, transform.localScale, offset);
        }
    }

    private Vector2 CreateGrid(GameObject obj, Vector3 size, Vector3 offset = default)
    {
        Vector3 objectSize = obj.GetSize();

        GameObject root = null;
        try
        {
            root = transform.GetChild(0).gameObject;
        }
        catch
        {
            root = new GameObject(gridRootName);
        }

        Vector3 origin = root.transform.position = transform.position + -(gridOrigin / 2);
        root.transform.SetParent(transform);

        origin -= new Vector3((objectSize.x / 2) + offset.x, 0, -(objectSize.z / 2) + offset.z);

        for (int x = 0; x < (int)size.x; x++)
        {
            origin.x += objectSize.x + offset.x;
            origin.z = root.transform.position.z - ((objectSize.z / 2) + offset.z);

            for (int z = 0; z < (int)size.z; z++)
            {
                origin.z += objectSize.z + offset.z;
                Vector3 position = new Vector3(origin.x, 0, origin.z);
                Instantiate(obj, position, Quaternion.identity, root.transform);
            }
        }

        currentGrid.Add(root);
        return origin;
    }

    private void ClearGrid(List<GameObject> grid)
    {
        List<GameObject> temporary = new List<GameObject>(grid);
        foreach (var tile in temporary)
        {
            grid.Remove(tile);
            DestroyImmediate(tile);
        }

        grid.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, gridOrigin);
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(transform.position + -gridOrigin / 2, .5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "grid.png");
    }
}
