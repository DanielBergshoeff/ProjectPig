using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour
{
    public GameObject tile;
    public GameObject boundaries;

    [HideInInspector]
    public Vector2 _size
    {
        get { return size; }
        set { updateGrid = true; }
    }

    [SerializeField]
    private Vector2 size = new Vector2();
    private bool updateGrid = false;

    private List<GameObject> grid = new List<GameObject>();

    private void OnValidate()
    {
        _size = size;
    }

    private void Update()
    {
        if (!updateGrid) { return; }

        if (!tile && !boundaries)
        {
            print("Please give a tile and boundaries object");
            updateGrid = false;
            return;
        }

        ClearGrid(grid);

        Vector3 gridObjectSize = tile.GetSize();
        Vector3 boundariesObjectSize = boundaries.GetSize();

        CreateGrid(tile, gridObjectSize);
        CreateBoundaries(boundaries, boundariesObjectSize);

        updateGrid = false;
    }

    private GameObject CreateGrid(GameObject obj, Vector3 size)
    {
        GameObject root = new GameObject("GridRoot");
        root.transform.SetParent(transform);
        float posX = transform.position.x - (size.x / 2);

        for (int x = 0; x < (int)_size.x; x++)
        {

            float posZ = transform.position.z - (size.z / 2);
            posX += size.x;

            for (int z = 0; z < (int)_size.y; z++)
            {
                posZ += size.z;
                Vector3 position = new Vector3(posX, 0, posZ);
                Instantiate(obj, position, Quaternion.identity, root.transform);
            }
        }

        grid.Add(root);
        return root;
    }

    private GameObject CreateBoundaries(GameObject obj, Vector3 size)
    {
        GameObject root = new GameObject("BoundaryRoot");
        root.transform.SetParent(transform);

        Quaternion rotation = Quaternion.identity;
        float posX = transform.position.x - (size.x / 2);
        float posZ = transform.position.z - (size.z / 2);

        for (int x = 0; x < (int)_size.x; x++)
        {
            posX += size.x;
            Vector3 position = new Vector3(posX, 0, posZ);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(90, Vector3.up);
        posX += (size.x / 2);
        posZ -= (size.x / 2);

        for (int y = 0; y < (int)_size.y; y++)
        {
            posZ += size.x;
            Vector3 position = new Vector3(posX, 0, posZ);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(0, Vector3.up);
        posX += (size.x / 2);
        posZ -= (size.x / 2);

        for (int x = 0; x < (int)_size.x; x++)
        {
            posX -= size.x;
            Vector3 position = new Vector3(posX, 0, posZ);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(90, Vector3.up);
        posX -= (size.x / 2);
        posZ += (size.x / 2);

        for (int y = 0; y < (int)_size.y; y++)
        {
            posZ -= size.x;
            Vector3 position = new Vector3(posX, 0, posZ);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(0, Vector3.up);

        grid.Add(root);
        return root;
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
}
