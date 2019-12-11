using TMPro;
using UnityEngine;

public class DialogueTriggerObject : MonoBehaviour, IInteractible
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;

    public void Interact()
    {
        TriggerDialogue();
    }

    private void Start()
    {
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
            DialogueManager.Instance.StartDialogue(dialogue, tMP_Text.gameObject);
        }
        DialogueManager.Instance.StartDialogue(dialogue);

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
