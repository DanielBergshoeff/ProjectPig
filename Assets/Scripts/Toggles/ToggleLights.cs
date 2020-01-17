using UnityEngine;

public class ToggleLights : MonoBehaviour, IIntractable
{
    [SerializeField] private GameObject pathLock;
    [SerializeField] private SetupPigAnimations pigAnims;
    private Light[] lights;
    public bool interacted { get; set; }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        int childCount = transform.parent.childCount;

        lights = transform.parent.GetComponentsInChildren<Light>();
        foreach (var light in lights) {
            light.enabled = false;
        }
    }

    public void Interact()
    {
        pathLock.SetActive(!pathLock.activeSelf);
        pigAnims.SwitchToScreaming();

        foreach (var light in lights)
        {
            light.enabled = !light.enabled;
        }
    }
}
