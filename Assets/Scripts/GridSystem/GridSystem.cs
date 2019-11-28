using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour
{
    [HideInInspector]
    public List<GridObject> _grids
    {
        get { return grids; }
        set { updateGrid = true; }
    }

    [SerializeField]
    private List<GridObject> grids = new List<GridObject>();

    private bool updateGrid = false;
    private List<GameObject> currentGrid = new List<GameObject>();

    private const string gridRootName = "GridRoot";
    private const string boundaryRootName = "BoundaryRoot";

    private void OnValidate()
    {
        _grids = grids;
    }

    private void Awake()
    {
        foreach (Transform child in transform)
            if (child.name == gridRootName || child.name == boundaryRootName)
                currentGrid.Add(child.gameObject);
    }

    private void Update()
    {
        if (!updateGrid) { return; }

        ClearGrid(currentGrid);

        Vector2 point = new Vector2();

        foreach (GridObject grid in grids)
        {
            if (!grid.tile)
            {
                print("Please give a tile object");
                updateGrid = false;
                return;
            }

            point = CreateGrid(grid.tile, grid.size, point);

            if (grid.boundaries)
                CreateBoundaries(grid.boundaries, grid.size, point);
        }
        updateGrid = false;
    }

    private Vector2 CreateGrid(GameObject obj, Vector3 size, Vector2 origin = default)
    {
        Vector3 objectSize = obj.GetSize();

        GameObject root = new GameObject(gridRootName);
        root.transform.SetParent(transform);
        origin.x = transform.position.x - (objectSize.x / 2);

        for (int x = 0; x < (int)size.x; x++)
        {

            origin.y = transform.position.y - (objectSize.z / 2);
            origin.x += objectSize.x;

            for (int z = 0; z < (int)size.y; z++)
            {
                origin.y += objectSize.z;
                Vector3 position = new Vector3(origin.x, 0, origin.y);
                Instantiate(obj, position, Quaternion.identity, root.transform);
            }
        }

        currentGrid.Add(root);
        return origin;
    }

    private Vector2 CreateBoundaries(GameObject obj, Vector3 size, Vector2 origin = default)
    {
        Vector3 objectSize = obj.GetSize();

        GameObject root = new GameObject(boundaryRootName);
        root.transform.SetParent(transform);

        Quaternion rotation = Quaternion.identity;
        // origin.x = origin.x - (objectSize.x / 2);
        // origin.y = origin.y - (objectSize.z / 2);

        for (int x = 0; x < (int)size.x; x++)
        {
            origin.x += objectSize.x;
            Vector3 position = new Vector3(origin.x, 0, origin.y);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(90, Vector3.up);
        origin.x += (objectSize.x / 2);
        origin.y -= (objectSize.x / 2);

        for (int y = 0; y < (int)size.y; y++)
        {
            origin.y += objectSize.x;
            Vector3 position = new Vector3(origin.x, 0, origin.y);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(0, Vector3.up);
        origin.x += (objectSize.x / 2);
        origin.y -= (objectSize.x / 2);

        for (int x = 0; x < (int)size.x; x++)
        {
            origin.x -= objectSize.x;
            Vector3 position = new Vector3(origin.x, 0, origin.y);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(90, Vector3.up);
        origin.x -= (objectSize.x / 2);
        origin.y += (objectSize.x / 2);

        for (int y = 0; y < (int)size.y; y++)
        {
            origin.y -= objectSize.x;
            Vector3 position = new Vector3(origin.x, 0, origin.y);
            Instantiate(obj, position, rotation, root.transform);
        }

        rotation = Quaternion.AngleAxis(0, Vector3.up);

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
}
