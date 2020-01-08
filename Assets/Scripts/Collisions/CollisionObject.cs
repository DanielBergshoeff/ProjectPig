using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionObject : MonoBehaviour
{
    public CollisionEvent collisionEvent;
    public TriggerEvent triggerEvent;

    private void OnCollisionEnter(Collision collision) {
        if(collisionEvent != null)
            collisionEvent.Invoke(collision, true);
    }

    private void OnCollisionExit(Collision collision) {
        if(collisionEvent != null)
            collisionEvent.Invoke(collision, false);
    }

    private void OnTriggerEnter(Collider other) {
        if(triggerEvent != null)
            triggerEvent.Invoke(other, true);
    }

    private void OnTriggerExit(Collider other) {
        if(triggerEvent != null)
            triggerEvent.Invoke(other, false);
    }
}

public class CollisionEvent : UnityEvent<Collision, bool> { }

public class TriggerEvent : UnityEvent<Collider, bool> { }
