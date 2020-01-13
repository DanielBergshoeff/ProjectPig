﻿using System;
using UnityEngine;

public class BystanderController : MonoBehaviour
{
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private KeyCode interactionKey = KeyCode.Mouse0;

    [Header("Flashlight")]
    [SerializeField] private GameObject flashlightObject;
    [SerializeField] private KeyCode flashlightKey = KeyCode.F;

    private GameObject interactionCanvas;

    private void Awake()
    {
        interactionCanvas = GetComponentInChildren<Canvas>().transform.GetChild(0).gameObject;
        interactionCanvas.SetActive(false);
    }

    private void Update()
    {
        CheckForInteraction();
        CheckForFleshlight();

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDistance, Color.red);
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
            interactionCanvas.SetActive(false);
            return;
        }

        if (!(hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable")))
            return;

        IInteractible interactible = hit.collider.GetComponent<IInteractible>();

        if (interactible == null) {
            interactionCanvas.SetActive(false);
            return;
        }

        interactionCanvas.SetActive(true);

        if (!Input.GetKeyDown(KeyCode.Mouse0)) { return; }

        print("click");

        interactible.Interact();
    }
}
