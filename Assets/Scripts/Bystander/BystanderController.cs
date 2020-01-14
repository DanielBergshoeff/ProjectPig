﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class BystanderController : MonoBehaviour
{
    [Header("Flashlight")]
    [SerializeField] private GameObject flashlightObject;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;

    [Header("Move tutorial")]
    [SerializeField] private GameObject moveTutorialCanvas;
    private Image imageMoveTutorial;
    [SerializeField] private bool moveTutorial = false;
    [SerializeField] private float moveTutorialTimer = 5.0f;

    [Header("Interact")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.Mouse0;
    [SerializeField] private float interactionTimer = 1.0f;

    private bool wasdUsed = false;
    private bool mouseMoved = false;
    private float moveTutorialTotalTime = 0f;
    private GameObject interactionCanvas;
    private bool interactionCanvasOn = false;
    private float interactionTimerTotal = 0f;
    private Image interactionImage;

    private void Awake()
    {
        interactionCanvas = GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
        interactionCanvas.SetActive(true);

        interactionImage = interactionCanvas.GetComponentInChildren<Image>();
        Color tempColor = interactionImage.color;
        tempColor.a = 0f;
        interactionImage.color = tempColor;

        imageMoveTutorial = moveTutorialCanvas.GetComponent<Image>();
        moveTutorialTotalTime = moveTutorialTimer;
        interactionTimerTotal = interactionTimer;

        if (moveTutorial)
            EnableMoveTutorial();
    }

    private void Update()
    {
        CheckForInteraction();
        CheckForFleshlight();

        if (moveTutorial)
            UpdateMoveTutorial();

        if (!moveTutorial)
            DisableMoveTutorial();

        UpdateInteractionCanvas();

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDistance, Color.red);
    }

    private void UpdateInteractionCanvas() {
        if (interactionCanvasOn && interactionImage.color.a < 1f) {
            interactionTimer -= Time.deltaTime;
            if (interactionTimer < 0f)
                interactionTimer = 0f;
        }
        else if(!interactionCanvasOn && interactionImage.color.a > 0f) {
            interactionTimer += Time.deltaTime;
            if (interactionTimer > interactionTimerTotal)
                interactionTimer = interactionTimerTotal;
        }
        else {
            return;
        }

        Color tempColor = interactionImage.color;
        tempColor.a = 1f - interactionTimer / interactionTimerTotal;
        interactionImage.color = tempColor;
    }

    private bool HasMouseMoved() {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    private bool HasUsedWASD() {
        return (Input.GetKeyDown(KeyCode.W));
    }

    private void EnableMoveTutorial() {
        moveTutorialCanvas.SetActive(true);
    }

    private void UpdateMoveTutorial() {
        if (!wasdUsed && HasUsedWASD())
            wasdUsed = true;
        if (!mouseMoved && HasMouseMoved())
            mouseMoved = true;

        if (wasdUsed && mouseMoved) {
            Invoke("DisableMoveTutorial", 5.0f);
            moveTutorial = false;
        }
    }

    private void DisableMoveTutorial() {
        if (moveTutorialTimer > 0)
            moveTutorialTimer -= Time.deltaTime;
        if (moveTutorialTimer < 0f)
            moveTutorialTimer = 0f;
        Color temp = imageMoveTutorial.color;
        temp.a = moveTutorialTimer / moveTutorialTotalTime;
        imageMoveTutorial.color = temp;
    }

    private void CheckForFleshlight()
    {
        if (Input.GetKeyDown(flashlightKey))
            flashlightObject.SetActive(!flashlightObject.activeSelf);
    }

    private void CheckForInteraction()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance))
        {
            interactionCanvasOn = false;
            return;
        }

        IInteractible interactible = hit.collider.GetComponent<IInteractible>();

        if (interactible == null) {
            interactionCanvasOn = false;
            return;
        }

        //interactionCanvas.SetActive(true);
        interactionCanvasOn = true;

        if (!Input.GetKeyDown(interactionKey)) { return; }

        print("click");

        interactible.Interact();
    }
}
