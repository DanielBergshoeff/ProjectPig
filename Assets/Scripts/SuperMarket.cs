using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperMarket : MonoBehaviour
{
    [SerializeField] private Animator elevatorWallAnimator;
    [SerializeField] private Animator elevatorAnimator;

    private bool shown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ElevatorOpening() {
        if (shown)
            return;

        elevatorWallAnimator.SetTrigger("Up");
        Invoke("OpenElevator", 3.0f);

        shown = true;
    }

    private void OpenElevator() {
        elevatorAnimator.SetTrigger("Open");
    }
}
