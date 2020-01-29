using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

// #if UNITY_EDITOR
// using UnityEditor.SceneManagement;
// [ExecuteInEditMode]
// #endif
public class LoadScenes : MonoBehaviour
{
    [SerializeField] private string[] scenes;
    private void Awake()
    {
        foreach (string scene in scenes)
        {
            // #if UNITY_EDITOR
            //             if (!Application.isPlaying)
            //             {
            //                 EditorSceneManager.OpenScene(scene, OpenSceneMode.Additive);
            //                 EditorSceneManager.sceneSaved += OnSceneSaved;
            //             }
            //             else
            //             {
            // #endif
            string sceneName = Path.GetFileName(scene);
            SceneManager.LoadScene(sceneName.Split('.')[0], LoadSceneMode.Additive);
            // #if UNITY_EDITOR
            //             }
            // #endif
        }
    }

    // #if UNITY_EDITOR
    //     private void OnSceneSaved(Scene scene)
    //     {
    //         EditorSceneManager.SaveOpenScenes();
    //     }

    //     [ContextMenu("Test")]
    //     public void test()
    //     {
    //         Awake();
    //     }
    // #endif
}