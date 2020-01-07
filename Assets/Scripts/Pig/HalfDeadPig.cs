using System.Collections;
using UnityEngine;

public class HalfDeadPig : MonoBehaviour
{
    public float maxSpasms = 75f;
    public float forceMultiplier = 5f;
    public AudioClip[] screams;
    public Rigidbody[] rbs;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        Task fadeOut = new Task(AddRandomRotation());
    }

    IEnumerator AddRandomRotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 3));
            source.PlayOneShot(screams[UnityEngine.Random.Range(0, screams.Length)]);

            foreach (var rb in rbs)
            {
                rb.maxAngularVelocity = maxSpasms;
                Vector3 randomDirection = new Vector3().RandomVector(0f, 100f);
                rb.AddRelativeTorque((randomDirection * rb.mass) * forceMultiplier, ForceMode.Force);
            }
        }
        yield return null;
    }
}
