using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDrownController : MonoBehaviour
{

    [SerializeField] private float distanceDown = 0.5f;
    [SerializeField] private float timeTillDown = 1.0f;

    [Header("Pig variables")]
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private Transform pigSpawnPosition;

    [Header("Drowning variables")]
    [SerializeField] private TextMeshProUGUI drownTimeText;
    [SerializeField] private float totalTimeDrown = 5.0f;
    [SerializeField] private CollisionObject drownTriggerObject;
    public GameObject Platform;

    [Header("Keycodes")]
    [SerializeField] private KeyCode keySpawn = KeyCode.Alpha1;
    [SerializeField] private KeyCode keyDown = KeyCode.Alpha2;
    [SerializeField] private KeyCode keyTilt = KeyCode.Alpha3;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool goingDown = false;
    private float goingDownTimer = 0f;
    private bool drowning = false;
    private float drownTimer = 0f;
    private bool drowned = false;

    private Animator gridAnimator;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = Platform.transform.position;
        endPosition = startPosition - Vector3.up * distanceDown;
        if (drownTriggerObject.triggerEvent == null)
            drownTriggerObject.triggerEvent = new TriggerEvent();
        drownTriggerObject.triggerEvent.AddListener(DrownPig);

        drownTimer = totalTimeDrown;
        gridAnimator = Platform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keySpawn)) {
            SpawnPig();
        }

        if (Input.GetKeyDown(keyTilt)) {
            gridAnimator.SetTrigger("Tilt");
        }

        MovePlatform();


        if (drowned)
            return;

        if (drowning && drownTimer > 0f) {
            drownTimer -= Time.deltaTime;
            if (drownTimer <= 0f) {
                drownTimer = 0f;
                drowned = true;
            }
        }
        else if (!drowning && drownTimer < totalTimeDrown) {
            drownTimer += Time.deltaTime;
            if (drownTimer > totalTimeDrown)
                drownTimer = totalTimeDrown;
        }

        drownTimeText.text = drownTimer.ToString("F2");
    }


    /// <summary>
    /// Move platform based on user input
    /// </summary>
    private void MovePlatform() {
        if (Input.GetKeyDown(keyDown)) {
            goingDown = true;
        }
        if (Input.GetKeyUp(keyDown)) {
            goingDown = false;
        }

        if (goingDown && goingDownTimer < timeTillDown)
            goingDownTimer += Time.deltaTime;
        else if (!goingDown && goingDownTimer > 0f)
            goingDownTimer -= Time.deltaTime;

        Vector3 direction = (endPosition - startPosition);
        Platform.transform.position = startPosition + direction * goingDownTimer / timeTillDown;
    }


    /// <summary>
    /// Instantiate pig at the pig spawn position
    /// </summary>
    private void SpawnPig() {
        Instantiate(pigPrefab, pigSpawnPosition.transform.position, pigSpawnPosition.transform.rotation);
    }

    private void DrownPig(Collider other, bool enter) {
        if (!other.CompareTag("Pig")) 
            return;

        if (enter)
            drowning = true;
        else
            drowning = false;
    }
}
