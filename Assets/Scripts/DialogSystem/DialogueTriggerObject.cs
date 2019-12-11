using UnityEngine;

public class DialogueTriggerObject : MonoBehaviour, IInteractible
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (!TriggerByTouch)
            return;

        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    public void Interact()
    {
        TriggerDialogue();
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
