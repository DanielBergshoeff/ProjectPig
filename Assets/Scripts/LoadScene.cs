using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class LoadScene : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject transitionUI;
    [SerializeField] private float transitionOffset = 2f;
    [SerializeField] private AudioClip transitionInAudio;
    [SerializeField] private AudioClip transitionOutAudio;

    private Task fadeIn;
    private Task fadeOut;
    private GameObject transition;
    private CanvasGroup canvas;
    private AudioSource source;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        InnitLoadScene();
    }

    public void Interact()
    {
        InnitLoadScene();
    }

    private void InnitLoadScene()
    {
        string scene = gameObject.name;

        if (scene == "Exit")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }

        transition = Instantiate(transitionUI);
        DontDestroyOnLoad(transition);

        canvas = transition.GetComponentInChildren<CanvasGroup>();
        canvas.alpha = 0;

        source.clip = transitionInAudio;
        source.Play();

        fadeIn = new Task(FadeIn());

        fadeIn.Finished += delegate (bool manual)
        {
            Load(scene);
        };
    }

    private void Load(string scene)
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += Loaded;
        SceneManager.LoadSceneAsync(scene);
    }

    private void Loaded(Scene scene, LoadSceneMode mode)
    {
        fadeOut = new Task(FadeOut());

        source.Stop();
        source.clip = transitionOutAudio;
        source.Play();

        fadeOut.Finished += delegate (bool manual)
        {
            Destroy(transition);
            Destroy(gameObject);
        };
    }

    IEnumerator FadeIn()
    {
        for (int i = 0; i <= 10; i += 1)
        {
            float a = canvas.alpha;
            a = i / 10f;
            canvas.alpha = a;
            yield return null;
        }

        yield return new WaitForSeconds(transitionInAudio.length + transitionOffset);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(transitionOutAudio.length + transitionOffset);

        for (int i = 10; i >= 0; i -= 1)
        {
            float a = canvas.alpha;
            a = i / 10f;
            canvas.alpha = a;
            yield return null;
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "Exit.png", true);
    }
}
