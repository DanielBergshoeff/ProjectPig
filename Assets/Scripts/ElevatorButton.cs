using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, IInteractible
{
    public void Interact() {
        SuperMarket.Instance.CloseElevator();
    }
}
