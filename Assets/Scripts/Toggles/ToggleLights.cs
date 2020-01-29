using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleLights : MonoBehaviour
{
    [SerializeField] private GameObject pathLock;
    [SerializeField] private SetupPigAnimations pigAnims;
    [SerializeField] private GameObject path1;
    [SerializeField] private GameObject path2;
    [SerializeField] private Material lightMat;
    private Light[] lights;
    private GameObject[] lightsMat;
    public bool interacted { get; set; }

    private void Awake()
    {
        path1 = GameObject.Find("Part1");
        path2 = GameObject.Find("Part2");
        pigAnims = FindObjectOfType<SetupPigAnimations>();

        lightsMat = GameObject.FindGameObjectsWithTag("TubeLamp");

        gameObject.layer = LayerMask.NameToLayer("Interactable");

        int childCount = transform.parent.childCount;

        lights = GameObject.Find("BoarRoomLights").GetComponentsInChildren<Light>();
        foreach (var light in lights)
        {
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

        GameObject go1 = GameObject.FindGameObjectWithTag("Player");
        SceneManager.MoveGameObjectToScene(go1, SceneManager.GetSceneByName("BoarRoom"));

        SceneManager.UnloadSceneAsync("SupermarketRoom");
        SceneManager.UnloadSceneAsync("VoorKamerRoom");
        SceneManager.UnloadSceneAsync("DaderRoom");

        FindObjectOfType<LightmapsSwap>().SwapLightmaps();
        SceneManager.LoadSceneAsync("DaderRoom", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("VoorKamerRoom", LoadSceneMode.Additive);
        go1.GetComponent<BystanderController>().moveTutorial = false;

        foreach (GameObject go in lightsMat)
        {
            Renderer renderer1 = go.GetComponent<Renderer>();
            renderer1.material = lightMat;
        }

        foreach (var light in lights)
        {
            light.enabled = true;
            light.intensity = 1;
        }

        Done();
    }
    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
