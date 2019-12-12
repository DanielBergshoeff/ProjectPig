using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pig : MonoBehaviour
{
    public GameObject FixedJointObject;
    public bool Dehaired = false;
    public bool Checked = false;

    private FixedJoint myFixedJoint;
    private Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myFixedJoint = FixedJointObject.GetComponent<FixedJoint>();
        myRigidbody = FixedJointObject.GetComponent<Rigidbody>();
    }

    public void UnHook() {
        Destroy(myFixedJoint);
        Destroy(myRigidbody);
    }
}


