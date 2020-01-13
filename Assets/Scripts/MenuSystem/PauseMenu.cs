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
        cachedPauseUI = Instantiate(pauseUI);
        cachedPauseUI.SetActive(false);

        var pausables = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour pausable in pausables)
        {
            if (pausable == this) { continue; }
            print(pausable);
            objectToPause.Add(pausable);
        }

        cachedPauseUI = Instantiate(pauseUI);
        cachedPauseUI.GetComponentInChildren<Button>().onClick.AddListener(Pause);
        cachedPauseUI.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    public void Pause()
    {
        paused = TogglePause();
        ToggleMonoBehaviours();
    }

    private bool TogglePause()
    {
        Cursor.visible = !Cursor.visible;

        if (paused)
        {
            Time.timeScale = 1f;
            cachedPauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            cachedPauseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            return (true);
        }
    }

    private void ToggleMonoBehaviours()
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
