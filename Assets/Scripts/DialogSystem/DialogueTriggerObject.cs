using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTriggerObject : MonoBehaviour, IInteractible
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;
    [SerializeField] private UnityEvent dialogueMethod;
    [SerializeField] private ButtonAudio buttonAudio;

    public void Interact()
    {
        TriggerDialogue();
        if (buttonAudio == null)
            return;

        buttonAudio.PlayAudio();
    }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        GetComponent<Collider>().isTrigger = TriggerByTouch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!TriggerByTouch)
            return;

        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (dialogue == null) { Debug.LogError(gameObject.name + " has no dialogue object."); return; }

        if (dialogue.lines.Length < 1) { Debug.LogError(gameObject.name + " has no dialogue lines."); return; }

        TMP_Text tMP_Text = gameObject.GetComponentInChildren<TMP_Text>();

        if (tMP_Text)
        {
            DialogueManager.Instance.StartDialogue(dialogue, tMP_Text.gameObject, dialogueMethod);
        }
        DialogueManager.Instance.StartDialogue(dialogue, null, dialogueMethod);

    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (TriggerByTouch)
        {
            Gizmos.color = new Color(0, 0, 0.5f, 0.25f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
        Gizmos.DrawIcon(transform.position, "DialogueGizmo.png", true);
    }
}
