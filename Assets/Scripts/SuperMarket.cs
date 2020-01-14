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

    [SerializeField] private float timeInElevator = 6f;

    [SerializeField] private AudioSource elevatorAudio;
    [SerializeField] private AudioClip elevatorMovement;
    [SerializeField] private AudioClip elevatorOpenDoor;
    [SerializeField] private AudioClip elevatorButtonPress;
    [SerializeField] private AudioClip elevatorWallOpen;

    private AudioSource elevatorWallAudioSource;
    private bool shown = false;
    private bool moving = false;
    private bool down = false;
    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        originalPosition = elevator.transform.position;
        elevatorWallAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ElevatorOpening() {
        if (shown)
            return;

        elevatorWallAnimator.SetTrigger("Up");
        elevatorWallAudioSource.PlayOneShot(elevatorWallOpen);
        Invoke("OpenElevator", 3.0f);
        Invoke("StopElevatorWallOpenAudio", 5f);

        shown = true;
    }

    private void StopElevatorWallOpenAudio() {
        elevatorWallAudioSource.Stop();
    }

    public void OpenElevator() {
        elevatorAnimator.SetTrigger("Open");
        elevatorAudio.Stop();
        moving = false;
        elevatorAudio.PlayOneShot(elevatorOpenDoor);
    }

    public void CloseElevator() {
        if (moving)
            return;
        
        moving = true;
        elevatorAnimator.SetTrigger("Close");
        Invoke("SetNewElevatorPosition", 5f);
        elevatorAudio.PlayOneShot(elevatorOpenDoor);
    }

    public void SetNewElevatorPosition() {
        elevatorAudio.clip = elevatorMovement;
        elevatorAudio.loop = true;
        elevatorAudio.Play();
        Player.GetComponent<CharacterController>().enabled = false;
        Player.GetComponent<FirstPersonController>().enabled = false;
        Player.transform.SetParent(elevator.transform);
        if (down)
            elevator.transform.position = originalPosition;
        else
            elevator.transform.position = secondElevatorPosition.position;
        down = !down;
        Invoke("TurnOnMovement", 0.1f);
        Invoke("OpenElevator", timeInElevator);
    }

    private void TurnOnMovement() {
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<FirstPersonController>().enabled = true;
        Player.transform.SetParent(null);
    }
}
