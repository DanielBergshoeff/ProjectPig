using UnityEngine;

//This script enables an animation upon entering this object's trigger

public class TriggerAnimation : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().enabled = true;
        }
    }
}
