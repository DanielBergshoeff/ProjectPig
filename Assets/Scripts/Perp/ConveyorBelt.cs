using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private List<GameObject> itemsOnBelt;


    private void Start() {
        itemsOnBelt = new List<GameObject>();
    }

    private void Update()
    {

        foreach (GameObject go in itemsOnBelt) {
            go.transform.position += transform.forward * Time.deltaTime * 0.3f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Pig"))
        {
            itemOnBelt = other.gameObject;
        }*/

        if (!itemsOnBelt.Contains(other.gameObject)) {
            itemsOnBelt.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (itemsOnBelt.Contains(other.gameObject)) {
            itemsOnBelt.Remove(other.gameObject);
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + transform.forward * 1f, Vector3.one * 0.1f);
    }
}
