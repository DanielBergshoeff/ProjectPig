using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTriggerObject : MonoBehaviour, IIntractable
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;
    [SerializeField] private UnityEvent preDialogueMethod;
    [SerializeField] private UnityEvent dialogueMethod;
    [SerializeField] private ButtonAudio buttonAudio;

    public bool interacted { get; set; }

    public void Interact()
    {
        interacted = true;
        dialogueMethod.AddListener(() => { interacted = false; Done(); });

        TriggerDialogue();

        if (buttonAudio != null)
            buttonAudio.PlayAudio();

        if (preDialogueMethod != null)
            preDialogueMethod.Invoke();
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
            Interact();
        }
    }

    private void TriggerDialogue()
    {
        if (dialogue == null) { Debug.LogError(gameObject.name + " has no dialogue object."); return; }

        if (dialogue.lines.Length < 1) { Debug.LogError(gameObject.name + " has no dialogue lines."); return; }

        TMP_Text tMP_Text = gameObject.GetComponentInChildren<TMP_Text>();

        if (tMP_Text)
        {
            DialogueManager.Instance.StartDialogue(dialogue, gameObject, dialogueMethod);
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

    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
