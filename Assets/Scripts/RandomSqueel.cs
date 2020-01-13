using System.Collections;
using UnityEngine;

public class RandomSqueel : MonoBehaviour
{
    private AudioSource source;
    private AudioClip[] sounds;

    public void Innit(AudioSource audioSource, AudioClip[] audioClip)
    {
        source = audioSource;
        sounds = audioClip;
        source.clip = sounds[Random.Range(0, sounds.Length)];

        Task t = new Task(RandomSound());
    }
    IEnumerator RandomSound()
    {
        while (true)
        {
            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.volume = Random.Range(0.1f, 1);
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(Random.Range(5, 10));
        }
    }
}
