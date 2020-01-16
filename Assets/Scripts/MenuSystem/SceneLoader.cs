using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour, IIntractable
{
    [SerializeField] private GameObject transitionUI;
    [SerializeField] private float transitionOffset = 2f;
    [SerializeField] private AudioClip transitionInAudio;
    [SerializeField] private AudioClip transitionOutAudio;

    private Task fadeOut;
    private Task fadeIn;
    private static GameObject transition;
    private static CanvasGroup canvas;
    private static AudioSource source;
    private List<AudioSource> sources;

    public bool interacted { get; set; }

    private void Innit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        transition = Instantiate(transitionUI);
        transition.SetActive(false);

        canvas = transition.GetComponentInChildren<CanvasGroup>();
        canvas.alpha = 0;

        DontDestroyOnLoad(transition);
    }

    private void OnTriggerEnter(Collider col)
    {
        interacted = true;

        Innit();
        Interact();
    }

    public void Interact()
    {
        interacted = true;

        QuitGame(gameObject.name);
        Innit();
        StartSceneTransition();
    }

    private void StartSceneTransition()
    {
        transition.SetActive(true);

        sources = FindObjectsOfType<AudioSource>().ToList();
        sources.Remove(source);

        fadeOut = new Task(Fade(true));

        PlayAudio(transitionInAudio);

        fadeOut.Finished += LoadScene;
    }

    private void EndSceneTransition(Scene scene, LoadSceneMode mode)
    {
        sources = FindObjectsOfType<AudioSource>().ToList();
        sources.Remove(source);

        fadeIn = new Task(Fade(false));

        PlayAudio(transitionInAudio);

        fadeIn.Finished += CleanUp;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (source == null)
            source = transition.AddComponent<AudioSource>();

        source.enabled = true;
        source.Stop();
        source.clip = clip;
        source.Play();
    }

    private void LoadScene(bool manual)
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += EndSceneTransition;
        SceneManager.LoadScene(gameObject.name);
    }


    public void LoadScene(string scene, GameObject transitionObject)
    {
        transitionUI = transitionObject;
        gameObject.name = scene;
        Innit();
        StartSceneTransition();
    }

    private void CleanUp(bool manual)
    {
        try
        {
            Destroy(transition);
            Destroy(gameObject);
            interacted = false;
        }
        catch { }
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

    IEnumerator Fade(bool fadeIn, int iterations = 50)
    {
        float length = transitionOutAudio ? transitionOutAudio.length : 0;

        if (fadeIn)
            yield return new WaitForSeconds(length + transitionOffset);

        int dir = fadeIn ? 1 : -1;
        int start = fadeIn ? 0 : iterations;
        int end = fadeIn ? iterations : 0;

        for (int i = start; i <= end; i += dir)
        {
            float a = canvas.alpha;
            a = (float)i / (float)iterations;
            print(fadeIn);
            print((fadeIn ? "out " : "in ") + a);
            foreach (AudioSource s in sources)
                s.volume = a - 1f;

            canvas.alpha = a;
            yield return null;
        }

        if (!fadeIn)
            yield return new WaitForSeconds(length + transitionOffset);
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "Exit.png", true);
    }
}
