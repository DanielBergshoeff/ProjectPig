using UnityEngine;
using UnityEngine.Events;

public class ToggleDoor : MonoBehaviour, IIntractable
{
    public UnityEvent action;
    public bool interacted { get; set; }

    private void Start() { }

    public void Interact()
    {
        GetComponent<Collider>().enabled = false;
        action.Invoke();
        Done();
    }
    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
