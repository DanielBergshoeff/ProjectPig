using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionObject : MonoBehaviour
{
    public CollisionEvent collisionEvent;
    public TriggerEvent triggerEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        collisionEvent.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other) {
        triggerEvent.Invoke(other);
    }
}

public class CollisionEvent : UnityEvent<Collision> { }

public class TriggerEvent : UnityEvent<Collider> { }
