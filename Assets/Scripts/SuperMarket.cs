﻿using UnityEngine;
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
    [SerializeField] private AudioClip elevatorDing;

    [SerializeField] private GameObject elevatorButtonUp;
    [SerializeField] private GameObject elevatorButtonDown;

    [SerializeField] private Material normalMat;
    [SerializeField] private Material litMat;

    private AudioSource elevatorWallAudioSource;
    private AudioSource elevatorDingSource;

    private bool shown = false;
    private bool moving = false;
    private bool down = false;
    private Vector3 originalPosition;

    private bool inLift = false;

    private void Start()
    {
        Instance = this;
        originalPosition = elevator.transform.position;
        elevatorWallAudioSource = gameObject.AddComponent<AudioSource>();
        elevatorDingSource = elevator.AddComponent<AudioSource>();
    }

    public void ElevatorOpening()
    {
        if (shown)
            return;

        elevatorWallAnimator.SetTrigger("Up");
        elevatorWallAudioSource.PlayOneShot(elevatorWallOpen);
        Invoke("OpenElevator", 3.0f);
        Invoke("StopElevatorWallOpenAudio", 5f);

        shown = true;
    }

    private void StopElevatorWallOpenAudio()
    {
        elevatorWallAudioSource.Stop();
    }

    public void OpenElevator()
    {
        elevatorAnimator.SetTrigger("Open");
        elevatorAudio.Stop();
        moving = false;
        elevatorAudio.PlayOneShot(elevatorOpenDoor);

        if (!down)
        {
            elevatorButtonUp.GetComponent<MeshRenderer>().material = normalMat;
        }
        else
        {
            elevatorButtonDown.GetComponent<MeshRenderer>().material = normalMat;
        }
    }

    public void CloseElevator()
    {
        if (moving)
            return;

        moving = true;
        elevatorAnimator.SetTrigger("Close");
        Invoke("SetNewElevatorPosition", 5f);
        elevatorAudio.PlayOneShot(elevatorOpenDoor);
        elevatorDingSource.PlayOneShot(elevatorDing);

        if (down)
        {
            elevatorButtonUp.GetComponent<MeshRenderer>().material = litMat;
        }
        else
        {
            elevatorButtonDown.GetComponent<MeshRenderer>().material = litMat;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            inLift = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            inLift = false;
        }
    }

    public void SetNewElevatorPosition()
    {
        if(!inLift) {
            OpenElevator();
            return;
        }

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

    private void TurnOnMovement()
    {
        Player.GetComponent<CharacterController>().enabled = true;
        Player.GetComponent<FirstPersonController>().enabled = true;
        Player.transform.SetParent(null);
    }
}
