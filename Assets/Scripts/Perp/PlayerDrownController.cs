using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDrownController : MonoBehaviour
{
    public static PlayerDrownController Instance { get; private set; }

    [SerializeField] private float distanceDown = 0.5f;
    [SerializeField] private float timeTillDown = 1.0f;

    [Header("Drowning variables")]
    [SerializeField] private TextMeshProUGUI drownTimeText;
    [SerializeField] private float totalTimeDrown = 5.0f;
    [SerializeField] private CollisionObject drownTriggerObject;
    public GameObject Platform;

    [Header("Hooks")]
    [SerializeField] private List<GameObject> hooks;

    [Header("Keycodes")]
    [SerializeField] private KeyCode keyDown = KeyCode.Alpha2;
    [SerializeField] private KeyCode keyTilt = KeyCode.Alpha3;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private bool goingDown = false;
    private float goingDownTimer = 0f;
    private bool drowning = false;
    private float drownTimer = 0f;

    private bool showTimer = true;

    private DehairingPig pigBeingDrowned;

    private Animator gridAnimator;

    private bool tilting = false;

    // Start is called before the first frame update
    void Start() {
        startPosition = Platform.transform.position;
        endPosition = startPosition - Vector3.up * distanceDown;
        if (drownTriggerObject.triggerEvent == null)
            drownTriggerObject.triggerEvent = new TriggerEvent();
        drownTriggerObject.triggerEvent.AddListener(DrownPig);

        drownTimer = totalTimeDrown;
        drownTimeText.text = drownTimer.ToString("F2");

        gridAnimator = Platform.GetComponent<Animator>();

        Instance = this;
    }

    private void UnTilt() {
        tilting = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(keyTilt) && goingDownTimer <= 0 && !tilting) {
            gridAnimator.SetTrigger("Tilt");
            tilting = true;
            Invoke("UnTilt", 2.5f);
        }

        MovePlatform();

        if (pigBeingDrowned == null || pigBeingDrowned.Dehaired)
            return;

        if (drowning && drownTimer > 0f) {
            drownTimer -= Time.deltaTime;
            if (drownTimer <= 0f) {
                drownTimer = 0f;
                pigBeingDrowned.Dehaired = true;
            }
        }
        else if (!drowning && drownTimer < totalTimeDrown) {
            drownTimer += Time.deltaTime;
            if (drownTimer > totalTimeDrown)
                drownTimer = totalTimeDrown;
        }

        if (!showTimer)
            return;
        drownTimeText.text = drownTimer.ToString("F2");
    }

    public void ResetDrown() {
        drownTimer = totalTimeDrown;

        if (!showTimer)
            return;
        drownTimeText.text = drownTimer.ToString("F2");
    }

    public void DisableTimer() {
        showTimer = false;
        drownTimeText.text = "";
    }


    /// <summary>
    /// Move platform based on user input
    /// </summary>
    private void MovePlatform() {
        if (tilting)
            return;

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

    private void DrownPig(Collider other, bool enter) {
        if (!other.CompareTag("Pig")) 
            return;

        if (enter) {
            drowning = true;
            pigBeingDrowned = other.GetComponentInParent<DehairingPig>();
        }
        else {
            drowning = false;
            pigBeingDrowned = null;
        }
    }
}
