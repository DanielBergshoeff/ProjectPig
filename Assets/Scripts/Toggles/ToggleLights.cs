using UnityEngine;

public class ToggleLights : MonoBehaviour, IInteractible
{
    private Light[] lights;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        int childCount = transform.parent.childCount;

        lights = transform.parent.GetComponentsInChildren<Light>();
    }

    public void Interact()
    {
        foreach (var light in lights)
        {
            light.enabled = !light.enabled;

        }
    }
}
