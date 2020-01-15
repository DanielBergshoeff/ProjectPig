using System.Collections;
using UnityEngine;

public class RandomSqueel : MonoBehaviour
{
    private AudioSource source;
    private AudioClip[] sounds;
    private Vector2 waitingRange;
    private Vector2 volumeRange;

    public void Innit(AudioSource audioSource, AudioClip[] audioClips, Vector2 waitForSecondsRange, Vector2 volRange)
    {
        source = audioSource;
        sounds = audioClips;
        source.clip = sounds[Random.Range(0, sounds.Length)];

        waitingRange = waitForSecondsRange;
        volumeRange = volRange;

        Task t = new Task(RandomSound());
    }
    public void SetSounds(AudioClip[] audioClips, Vector2 waitForSecondsRange, Vector2 volRange) {
        sounds = audioClips;
        waitingRange = waitForSecondsRange;
        volumeRange = volRange;
    }

    IEnumerator RandomSound()
    {
        while (true)
        {
            source.clip = sounds[Random.Range(0, sounds.Length)];
            source.volume = Random.Range(volumeRange.x, volumeRange.y);
            source.loop = false;
            source.Play();
            yield return new WaitForSeconds(Random.Range(waitingRange.x, waitingRange.y));
        }
    }
}
