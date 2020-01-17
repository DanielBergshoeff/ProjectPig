using UnityEngine;
using UnityEngine.Events;

public class ToggleDoor : MonoBehaviour, IIntractable
{
    public UnityEvent action;
    public bool interacted { get; set; }

    private void Start() { }

    public void Interact()
    {
        action.Invoke();
    }
}
