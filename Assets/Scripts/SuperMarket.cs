using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class SuperMarket : MonoBehaviour
{
    public static SuperMarket Instance { get; private set; }

    [SerializeField] private GameObject Player;

    [SerializeField] private Animator elevatorWallAnimator;
    [SerializeField] private Animator elevatorAnimator;

    [SerializeField] private GameObject elevator;
    [SerializeField] private Transform secondElevatorPosition;

    private bool shown = false;
    private bool down = false;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        originalPosition = transform.position;
    }

    public void ElevatorOpening() {
        if (shown)
            return;

        elevatorWallAnimator.SetTrigger("Up");
        Invoke("OpenElevator", 3.0f);

        shown = true;
    }

    public void OpenElevator() {
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<FirstPersonController>().enabled = true;
        Player.transform.SetParent(null);
        elevatorAnimator.SetTrigger("Open");
    }

    public void CloseElevator() {
        elevatorAnimator.SetTrigger("Close");
        Invoke("SetNewElevatorPosition", 3f);
    }

    public void SetNewElevatorPosition() {
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<FirstPersonController>().enabled = false;
        Player.transform.SetParent(elevator.transform);
        if (down)
            elevator.transform.position = originalPosition;
        else
            elevator.transform.position = secondElevatorPosition.position;
        Invoke("OpenElevator", 1f);
    }
}
