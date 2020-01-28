using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
[ExecuteAlways]
public class LoadScenes : MonoBehaviour
{
    [SerializeField] private string[] scenes;
    private void Awake()
    {
        foreach (string scene in scenes)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.OpenScene(scene, OpenSceneMode.Additive);
                EditorSceneManager.sceneSaved += OnSceneSaved;
            }
#elif UNITY_STANDALONE_WIN
            string sceneName = Path.GetFileName(scene);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
#endif
        }
    }

#if UNITY_EDITOR
    private void OnSceneSaved(Scene scene)
    {
        EditorSceneManager.SaveOpenScenes();
    }

    [ContextMenu("Test")]
    public void test()
    {
        Awake();
    }
#endif
}
