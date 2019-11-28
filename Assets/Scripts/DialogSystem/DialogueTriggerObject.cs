using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerObject : MonoBehaviour
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;


    public void TriggerDialogue() {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other) {
        if (!TriggerByTouch)
            return;

        if (other.CompareTag("Player")) {
            TriggerDialogue();
        }
    }
}
