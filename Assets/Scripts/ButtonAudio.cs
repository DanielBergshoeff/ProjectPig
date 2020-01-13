using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    private AudioSource myAudioSource;
    [SerializeField] private AudioClip buttonClip;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayAudio() {
        myAudioSource.PlayOneShot(buttonClip);
    }
}
