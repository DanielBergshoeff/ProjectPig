using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridSystem : MonoBehaviour
{
    public GameObject gridObject;
    public Vector3 offset;
    private Vector3 Offset;
    private Vector3 gridOrigin;
    private Vector3 objectSize;

    private List<GameObject> currentGrid = new List<GameObject>();
    private const string gridRootName = "GridRoot";

    private void Awake()
    {
        GetCurrentGrid();
    }

    private void Update()
    {
        if (!transform.hasChanged) { return; }
        transform.hasChanged = false;

        if (!gridObject)
        {
            print("Please give a tile object");
            return;
        }

        //Clear the current grid
        ClearGrid(currentGrid);

        //Get the size of the object relative to height grid object
        objectSize = ScaleYProportional(transform.localScale, gridObject.GetSize());

        //Calculate amount of objects in grid
        Vector3 gridAmount = GetAmount(transform.localScale, objectSize);
        print(gridAmount);

        //Get the grid origin
        float x = transform.position.x + (transform.localScale.x * offset.x);
        float y = transform.position.y + (transform.localScale.y * offset.y);
        float z = transform.position.z + (transform.localScale.z * offset.z);
        gridOrigin = new Vector3(x, y, z);

        //Create the grid
        CreateGrid(gridObject, gridOrigin, gridAmount, offset);
    }

    private Vector3 GetAmount(Vector3 gridSize, Vector3 objectSize)
    {
        return new Vector3(Mathf.RoundToInt(gridSize.x / objectSize.x), 1, Mathf.RoundToInt(gridSize.z / objectSize.z));
    }

    private void CreateGrid(GameObject obj, Vector3 position, Vector3 gridAmount, Vector3 offset = default)
    {
        //Setting the origin point from where the grid is going to be made
        Vector3 origin = position;

        //Setting the first position of the grid
        origin -= (objectSize / 2) + offset;
        for (int x = 0; x < gridAmount.x; x++)
        {
            origin.x += objectSize.x + offset.x;
            origin.z = position.z;

            for (int z = 0; z < gridAmount.z; z++)
            {
                origin.z += objectSize.z + offset.z;
                currentGrid.Add(CreateObject(obj, origin));
            }
        }
    }

    private GameObject CreateObject(GameObject obj, Vector3 position)
    {
        position = new Vector3(position.x, position.y, position.z);
        GameObject go = Instantiate(obj, position, Quaternion.identity);

        Vector3 scale = objectSize;
        go.transform.localScale = scale;

        go.transform.SetParent(transform);
        return go;
    }

    private void GetCurrentGrid()
    {
        foreach (Transform child in transform.GetChild(0).transform)
            if (child.name == gridRootName)
                currentGrid.Add(child.gameObject);
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

    private Vector3 ScaleYProportional(Vector3 toScale, Vector3 scaleTo)
    {
        return toScale / toScale.LongestAxis();
    }

    private float norm(float val, float min, float max)
    {
        return (val - min) / (max - min);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawSphere(gridOrigin, 0.2f);
        Gizmos.DrawIcon(transform.position, "grid.png");
    }
}
