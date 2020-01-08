using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SceneLoader : MonoBehaviour, IInteractible
{
    [SerializeField] private GameObject transitionUI;
    [SerializeField] private float transitionOffset = 2f;
    [SerializeField] private AudioClip transitionInAudio;
    [SerializeField] private AudioClip transitionOutAudio;

    private Task fadeIn;
    private Task fadeOut;
    private static GameObject transition;
    private static CanvasGroup canvas;
    private static AudioSource source;

    private void Innit()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");

        transition = Instantiate(transitionUI);
        transition.SetActive(false);

        source = transition.GetComponent<AudioSource>();
        canvas = transition.GetComponentInChildren<CanvasGroup>();
        canvas.alpha = 0;

        DontDestroyOnLoad(transition);
    }

    private void OnTriggerEnter(Collider col)
    {
        Interact();
    }

    public void Interact()
    {
        QuitGame(gameObject.name);
        Innit();
        StartSceneTransition();
    }

    private void StartSceneTransition()
    {
        transition.SetActive(true);

        fadeIn = new Task(FadeOut());

        PlayAudio(transitionInAudio);

        fadeIn.Finished += LoadScene;
    }

    private void EndSceneTransition(Scene scene, LoadSceneMode mode)
    {
        fadeOut = new Task(FadeIn());

        PlayAudio(transitionInAudio);

        fadeOut.Finished += CleanUp;
    }

    private void PlayAudio(AudioClip clip)
    {
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

    private void CleanUp(bool manual)
    {
        try
        {
            Destroy(transition);
            Destroy(gameObject);
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

    IEnumerator FadeOut()
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

    IEnumerator FadeIn()
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
