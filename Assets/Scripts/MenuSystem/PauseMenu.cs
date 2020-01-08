using UnityEngine;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    private bool paused = false;
    private List<MonoBehaviour> objectToPause = new List<MonoBehaviour>();
    private GameObject cachedPauseUI;

    private void Start()
    {
        var pausables = FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour pausable in pausables)
        {
            if (pausable == this) { continue; }
            if (pausable is UnityEngine.UI.GraphicRaycaster) { continue; }
            if (pausable is UnityEngine.UI.CanvasScaler) { continue; }
            if (pausable is UnityEngine.EventSystems.EventSystem) { continue; }
            if (pausable is UnityEngine.EventSystems.StandaloneInputModule) { continue; }

            print(pausable.GetType());
            objectToPause.Add(pausable);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            paused = TogglePause();
    }

    public void Pause()
    {
        paused = TogglePause();
    }

    private bool TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Destroy(cachedPauseUI);
            // ToggleMonoBehaviors();
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            cachedPauseUI = Instantiate(pauseUI);
            // ToggleMonoBehaviors();
            return (true);
        }
    }

    private void ToggleMonoBehaviors()
    {
        var copy = new List<MonoBehaviour>(objectToPause);
        foreach (MonoBehaviour pausable in copy)
        {
            if (pausable == default) { objectToPause.Remove(pausable); continue; }

            try
            {
                pausable.enabled = !pausable.enabled;
            }
            catch
            {
                pausable.enabled = true;
                Debug.LogWarning("Something went wrong but it's no problem.");
            }
        }
    }
}
