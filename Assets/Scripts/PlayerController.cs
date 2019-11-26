using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Hand;

    [SerializeField] private float handDistanceNormal;
    [SerializeField] private float handDistanceStretched;
    [SerializeField] private float stretchSpeed;
    [SerializeField] private float handReach;
    [SerializeField] private float handHeight;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform railA;
    [SerializeField] private Transform railB;

    private float handDistance;
    private bool stretching;
    private Camera cam;

    private GameObject heldPig;

    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        Cursor.visible = false;
        handDistance = handDistanceNormal;
    }

    // Update is called once per frame
    void Update()
    {
        PositionHand();
        RotatePlayer();
        MovePlayer();
    }

    private void MovePlayer() {
        if (Input.GetKey(KeyCode.A)) {
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, railA.transform.position, Time.deltaTime * moveSpeed);
            transform.position = targetPosition;
        }
        if (Input.GetKey(KeyCode.D)) {
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, railB.transform.position, Time.deltaTime * moveSpeed);
            transform.position = targetPosition;
        }
    }

    private void RotatePlayer() {
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(-Vector3.up * Time.deltaTime * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }

    /// <summary>
    /// Position and rotate the arm based on mouse position
    /// </summary>
    private void PositionHand() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Hand.transform.position = transform.position + Vector3.up * handHeight + ray.direction * handDistance;

        if (stretching && Input.GetMouseButtonUp(0)) {
            if(heldPig != null) {
                heldPig.transform.parent = null;
                heldPig.GetComponent<Rigidbody>().isKinematic = false;
                heldPig = null;
            }
        }

        stretching = Input.GetMouseButton(0);


        if (stretching && handDistance < handDistanceStretched) {
            handDistance += Time.deltaTime * stretchSpeed;
            if(handDistance >= handDistanceStretched) {
                GrabItem();
            }
        }

        else if (!stretching && handDistance > handDistanceNormal)
            handDistance -= Time.deltaTime * stretchSpeed;
    }

    /// <summary>
    /// Grab pig near hand
    /// </summary>
    private void GrabItem() {
        Collider[] hitColliders = Physics.OverlapSphere(Hand.transform.position, handReach);

        foreach(Collider c in hitColliders) {
            if (c.CompareTag("Pig")) {
                heldPig = c.gameObject;
                heldPig.GetComponent<Rigidbody>().isKinematic = true;
                heldPig.transform.parent = Hand.transform;
                return;
            }
        }
    }
}
