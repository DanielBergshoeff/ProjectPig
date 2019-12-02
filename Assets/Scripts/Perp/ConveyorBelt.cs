using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private GameObject itemOnBelt;

    private void Update()
    {
        if (itemOnBelt == null)
            return;

        itemOnBelt.transform.position += transform.forward * Time.deltaTime * 0.3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pig"))
        {
            itemOnBelt = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (itemOnBelt == null)
            return;

        if (itemOnBelt == other.gameObject)
            itemOnBelt = null;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + transform.forward * 1f, Vector3.one * 0.1f);
    }
}
