using UnityEngine;

public class ToggleLights : MonoBehaviour, IIntractable
{
    [SerializeField] private GameObject pathLock;
    private Light[] lights;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        int childCount = transform.parent.childCount;

        lights = transform.parent.GetComponentsInChildren<Light>();
    }

    public void Interact()
    {
        pathLock.SetActive(!pathLock.activeSelf);

        foreach (var light in lights)
        {
            light.enabled = !light.enabled;
        }
    }
}
