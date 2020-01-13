using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour, IInteractible
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
    private bool done = false;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    private void OnTriggerEnter(Collider col)
    {
        QuitGame(gameObject.name);
        InnitLoadScene(gameObject.name);
    }

    public void Interact()
    {
        QuitGame(gameObject.name);
        InnitLoadScene(gameObject.name);
    }

    private void InnitLoadScene(string sceneName)
    {
        transition = Instantiate(transitionUI);

        DontDestroyOnLoad(transition);

        canvas = transition.GetComponentInChildren<CanvasGroup>();
        canvas.alpha = 0;

        PlayAudio(transitionInAudio);

        fadeIn = new Task(FadeIn());

        fadeIn.Finished += delegate (bool manual)
        {
            Load(sceneName);
        };
    }

    private void PlayAudio(AudioClip ac) {
        if (source == null)
            source = transition.AddComponent<AudioSource>();

        source.Stop();
        source.clip = ac;
        source.Play();
    }

    private static void QuitGame(string sceneName)
    {
        if (sceneName != "Exit") { return; }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    private void Load(string sceneName)
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += Loaded;
        SceneManager.LoadSceneAsync(sceneName);
    }

    private void Loaded(Scene scene, LoadSceneMode mode)
    {
        fadeOut = new Task(FadeOut());

        PlayAudio(transitionOutAudio);

        fadeOut.Finished += CleanUp;
    }

    private void CleanUp(bool manual)
    {
        try
        {
            Destroy(transition);
            Destroy(gameObject);
        }
        catch { }
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

        float length = transitionOutAudio ? transitionInAudio.length : 0;
        yield return new WaitForSeconds(length + transitionOffset);
    }

    IEnumerator FadeOut()
    {
        float length = transitionOutAudio ? transitionInAudio.length : 0;
        yield return new WaitForSeconds(length + transitionOffset);

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
