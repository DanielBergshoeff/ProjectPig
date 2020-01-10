using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

            print(pausable.GetType());
            objectToPause.Add(pausable);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void Pause()
    {
        TogglePause();
    }

    private void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 1f;
            Destroy(cachedPauseUI);
            ToggleMonoBehaviors();
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            cachedPauseUI = Instantiate(pauseUI);
            cachedPauseUI.GetComponentInChildren<Button>().onClick.AddListener(TogglePause);
            ToggleMonoBehaviors();
            paused = true;
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
