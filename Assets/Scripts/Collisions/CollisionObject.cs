﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionObject : MonoBehaviour
{
    public CollisionEvent collisionEvent;
    public TriggerEvent triggerEvent;

    private void OnCollisionEnter(Collision collision) {
        collisionEvent.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other) {
        triggerEvent.Invoke(other, true);
    }

    private void OnTriggerExit(Collider other) {
        triggerEvent.Invoke(other, false);
    }
}

public class CollisionEvent : UnityEvent<Collision> { }

public class TriggerEvent : UnityEvent<Collider, bool> { }
