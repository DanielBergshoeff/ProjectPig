using UnityEngine;

public class ElevatorButton : MonoBehaviour, IIntractable
{
    [SerializeField] private bool multipleUse = false;

    public bool interacted { get; set; }

    public void Interact()
    {
        SuperMarket.Instance.CloseElevator();
        Done();
    }

    public void Done()
    {
        if (multipleUse)
            return;

        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
