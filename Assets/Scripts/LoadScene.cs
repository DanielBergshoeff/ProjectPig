using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        SceneManager.LoadScene(col.gameObject.name);
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.5f, 0, 0.25f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawIcon(transform.position, "Exit.png", true);
    }
}
