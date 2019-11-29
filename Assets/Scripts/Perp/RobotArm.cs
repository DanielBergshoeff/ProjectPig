using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotArm : MonoBehaviour
{
    public KeyCode InteractionKey;
    public GameObject Arm;
    public GameObject Pig;

    private bool clicking;
    private float animationTime;

    public AnimationClip AnimationGrab;
    private Animation myAnimation;
    private Animator myAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAnimation = GetComponent<Animation>();

        myAnimation.AddClip(AnimationGrab, AnimationGrab.name);

        if (Arm.GetComponent<CollisionObject>().collisionEvent == null)
            Arm.GetComponent<CollisionObject>().collisionEvent = new CollisionEvent();

        Arm.GetComponent<CollisionObject>().collisionEvent.AddListener(CollisionEnter);
    }

    // Update is called once per frame
    void Update()
    {
        clicking = Input.GetKey(InteractionKey);
        if (clicking) {
            MoveArmDown();
        }
        else {
            MoveArmUp();
        }
    }

    private void CollisionEnter(Collision coll) {
        if (coll.gameObject == Pig) {
            Pig.transform.SetParent(Arm.transform);
            Pig.GetComponent<Rigidbody>().isKinematic = true;
            Pig.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void MoveArmDown() {
        if (animationTime + Time.deltaTime <= AnimationGrab.length)
            animationTime += Time.deltaTime;
        else
            animationTime = AnimationGrab.length;

        myAnimation[AnimationGrab.name].time = animationTime;
        myAnimation[AnimationGrab.name].speed = 0;
        myAnimation.Play(AnimationGrab.name);
    }

    private void MoveArmUp() {
        if (animationTime - Time.deltaTime >= 0f)
            animationTime -= Time.deltaTime;
        else
            animationTime = 0f;

        myAnimation[AnimationGrab.name].time = animationTime;
        myAnimation[AnimationGrab.name].speed = 0;
        myAnimation.Play(AnimationGrab.name);
    }
}

