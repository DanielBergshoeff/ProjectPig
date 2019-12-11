using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;  private set; }

    public GameObject Player;

    private Vector3 startPosition;
    private float fixedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        startPosition = Player.transform.position;

        fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn() {
        StartCoroutine("RespawnAfterSlow");
    }

    private IEnumerator RespawnAfterSlow() {
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
