using UnityEngine;

public class ElevatorButton : MonoBehaviour, IIntractable
{
    public bool interacted { get; set; }

    public void Interact()
    {
        SuperMarket.Instance.CloseElevator();
        Done();
    }

    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
