using System.Collections;
using UnityEngine;

public class ForceRotation : MonoBehaviour
{
    public AudioClip[] screams;
    public Rigidbody[] rbs;
    private AudioSource source;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        foreach (var rb in rbs)
        {
            rb.maxAngularVelocity = 50f;
        }
        Task fadeOut = new Task(AddRandomRotation());
    }

    IEnumerator AddRandomRotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            source.PlayOneShot(screams[UnityEngine.Random.Range(0, screams.Length)]);
            foreach (var rb in rbs)
            {
                Vector3 randomDirection = new Vector3().RandomVector(0f, 100f);
                rb.AddRelativeTorque(randomDirection * rb.mass, ForceMode.Force);
            }
        }
        yield return null;
    }
}
