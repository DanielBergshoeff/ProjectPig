using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DehairingPig : MonoBehaviour
{
    public GameObject FixedJointObjectLeft;
    public GameObject FixedJointObjectRight;
    public bool Dehaired = false;
    public bool Checked = false;
    public bool Alive = false;

    private FixedJoint myFixedJointLeft;
    private Rigidbody myRigidbodyLeft;

    private FixedJoint myFixedJointRight;
    private Rigidbody myRigidbodyRight;

    // Start is called before the first frame update
    void Start()
    {
        myFixedJointLeft = FixedJointObjectLeft.GetComponent<FixedJoint>();
        myRigidbodyLeft = FixedJointObjectLeft.GetComponent<Rigidbody>();

        myFixedJointRight = FixedJointObjectRight.GetComponent<FixedJoint>();
        myRigidbodyRight = FixedJointObjectRight.GetComponent<Rigidbody>();
    }

    public void UnHook() {
        Destroy(myFixedJointLeft);
        Destroy(myRigidbodyLeft);

        Destroy(myFixedJointRight);
        Destroy(myRigidbodyRight);
    }

    public void Kill() {
        HalfDeadPig pig = GetComponent<HalfDeadPig>();
        if (pig == null)
            return;

        Destroy(pig.source);
        Destroy(pig);
    }
}


