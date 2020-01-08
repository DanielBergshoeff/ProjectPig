using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Pig : MonoBehaviour
{
    public List<AudioClip> PigSounds;
    public float minTimePerSound;
    public float maxTimePerSound;

    private bool playSound = true;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        if(PigSounds.Count > 0)
            Invoke("PlayPigSound", Random.Range(minTimePerSound, maxTimePerSound));
    }

    private void PlayPigSound() {
        myAudioSource.PlayOneShot(PigSounds[Random.Range(0, PigSounds.Count)]);
        Invoke("PlayPigSound", Random.Range(minTimePerSound, maxTimePerSound));
    }
}
