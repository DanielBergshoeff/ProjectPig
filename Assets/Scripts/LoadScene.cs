using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("LoadScene"))
        {
            SceneManager.LoadScene(col.gameObject.name);
        }
    }
}
