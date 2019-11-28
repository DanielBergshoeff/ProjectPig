using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BystanderController : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private KeyCode interactionKey;

    [SerializeField] private GameObject interactionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        interactionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteraction();
    }

    private void CheckForInteraction() {
        interactionCanvas.SetActive(false);
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance)) {
            if (hit.transform.CompareTag("Dialogue")) {

                DialogueTriggerObject dto = hit.transform.GetComponent<DialogueTriggerObject>();
                if (dto != null) {
                    if (!dto.TriggerByTouch) {
                        interactionCanvas.SetActive(true);
                        if (Input.GetKeyDown(interactionKey)) {
                            dto.TriggerDialogue();
                        }
                    }
                }
            }
        }
    }
}
