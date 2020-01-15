using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour, IIntractable
{
    public void Interact() {
        SuperMarket.Instance.CloseElevator();
    }
}
