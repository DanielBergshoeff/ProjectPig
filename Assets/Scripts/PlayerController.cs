using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Hand;

    [Header("Hand variables")]
    [SerializeField] private float handDistanceNormal = 0;
    [SerializeField] private float handDistanceStretched = 0;
    [SerializeField] private float stretchSpeed = 0;
    [SerializeField] private float handReach = 0;
    [SerializeField] private float handHeight = 0;

    [Header("Body variables")]
    [SerializeField] private float rotateSpeed = 0;
    [SerializeField] private float moveSpeed = 0;

    [Header("Rail variables")]
    [SerializeField] private Transform railA = default;
    [SerializeField] private Transform railB = default;

    [Header("Pig variables")]
    [SerializeField] private GameObject pigPrefab = default;
    [SerializeField] private Transform pigSpawnPosition = default;

    private float handDistance;
    private bool stretching;
    private Camera cam;

    private GameObject heldPig;
    private GameObject hangingPig;

    private float buttonTimer = 0f;
    private float buttonWaitTime = 3f;
    private bool buttonPressed = false;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        Cursor.visible = false;
        handDistance = handDistanceNormal;
        if (Hand.GetComponent<CollisionObject>() != null)
        {
            if (Hand.GetComponent<CollisionObject>().triggerEvent == null)
                Hand.GetComponent<CollisionObject>().triggerEvent = new TriggerEvent();

            Hand.GetComponent<CollisionObject>().triggerEvent.AddListener(HandTrigger);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PositionHand();
        RotatePlayer();
        MovePlayer();
    }

    /// <summary>
    /// Called when the hand touches another object
    /// </summary>
    /// <param name="other"></param>
    private void HandTrigger(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            ButtonPressed();
        }
    }

    private void ButtonPressed()
    {
        if (buttonPressed)
            return;

        if (hangingPig != null)
            return;

        buttonPressed = true;
        Invoke("ButtonUnPress", buttonWaitTime);
        hangingPig = Instantiate(pigPrefab);
        hangingPig.transform.position = pigSpawnPosition.position;
        hangingPig.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ButtonUnPress()
    {
        buttonPressed = false;
    }

    /// <summary>
    /// Move player based on A and D button press
    /// </summary>
    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, railA.transform.position, Time.deltaTime * moveSpeed);
            transform.position = targetPosition;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, railB.transform.position, Time.deltaTime * moveSpeed);
            transform.position = targetPosition;
        }
    }

    /// <summary>
    /// Rotate player based on A and D button press
    /// </summary>
    private void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }

    /// <summary>
    /// Position and rotate the arm based on mouse position
    /// </summary>
    private void PositionHand()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Hand.transform.position = transform.position + Vector3.up * handHeight + ray.direction * handDistance;

        if (stretching && Input.GetMouseButtonUp(0))
        {
            if (heldPig != null)
            {
                heldPig.transform.parent = null;
                heldPig.GetComponent<Rigidbody>().isKinematic = false;
                heldPig = null;
            }
        }

        stretching = Input.GetMouseButton(0);


        if (stretching && handDistance < handDistanceStretched)
        {
            handDistance += Time.deltaTime * stretchSpeed;
            if (handDistance >= handDistanceStretched)
            {
                GrabItem();
            }
        }

        else if (!stretching && handDistance > handDistanceNormal)
            handDistance -= Time.deltaTime * stretchSpeed;
    }

    /// <summary>
    /// Grab pig near hand
    /// </summary>
    private void GrabItem()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Hand.transform.position, handReach);

        foreach (Collider c in hitColliders)
        {
            if (c.CompareTag("Pig"))
            {
                heldPig = c.gameObject;
                heldPig.GetComponent<Rigidbody>().isKinematic = true;
                heldPig.transform.parent = Hand.transform;
                if (heldPig == hangingPig)
                    hangingPig = null;
                return;
            }
        }
    }
}
