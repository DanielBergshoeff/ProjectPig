using UnityEngine;

public class DialogueTriggerObject : MonoBehaviour
{
    public bool TriggerByTouch;

    [SerializeField] private DialogueObject dialogue;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
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

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
