using UnityEngine;

public class ToggleLights : MonoBehaviour, IIntractable
{
    [SerializeField] private GameObject pathLock;
    [SerializeField] private SetupPigAnimations pigAnims;
    [SerializeField] private GameObject path1;
    [SerializeField] private GameObject path2;
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

        path1.SetActive(true);
        path2.SetActive(false);
    }

    public void Interact()
    {
        path1.SetActive(false);
        path2.SetActive(true);

        pathLock.SetActive(!pathLock.activeSelf);
        pigAnims.SwitchToScreaming();

        foreach (var light in lights)
        {
            light.enabled = !light.enabled;
        }
        Done();
    }
    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
