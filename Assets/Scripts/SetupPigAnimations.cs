﻿using System.Linq;
using UnityEngine;

public class SetupPigAnimations : MonoBehaviour
{
    [SerializeField] private Vector2 animationSpeedRange = new Vector2(.5f, 1.5f);
    [SerializeField] private Vector2 colorTintRange = new Vector2(.75f, 1f);
    [SerializeField] private Vector2 sizeRange = new Vector2(.25f, 1.25f);
    [SerializeField] private Vector2 volumeRange = new Vector2(.25f, 1.25f);
    [SerializeField] private AudioClip[] pigScreaming;
    [SerializeField] private AudioClip[] pigSounds;
    [SerializeField] private Vector2 pigSoundWaitingRange = new Vector2(5f, 20f);
    [SerializeField] private Vector2 pigSoundVolumeRange = new Vector2(0.01f, 0.2f);
    [SerializeField] private Vector2 pigSoundVolumeRange2 = new Vector2(0.5f, 1f);
    private RuntimeAnimatorController[] animators;

    private bool switched = false;

    private void Start()
    {
        animators = Resources.LoadAll("PigAnimationControllers", typeof(RuntimeAnimatorController)).Cast<RuntimeAnimatorController>().ToArray();

        GameObject[] pigs = GameObject.FindGameObjectsWithTag("Pig");
        foreach (GameObject pig in pigs)
        {
            Animator animator = pig.gameObject.GetComponentInChildren<Animator>();
            if (!animator) { continue; }
            animator.runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
            float rndSize = Random.Range(animationSpeedRange.x, animationSpeedRange.y);
            animator.SetFloat("Speed", rndSize);

            rndSize = Random.Range(colorTintRange.x, colorTintRange.y);
            pig.transform.localScale *= rndSize;

            rndSize = Random.Range(sizeRange.x, sizeRange.y);
            Material mat = pig.GetComponentInChildren<Renderer>().material;
            mat.color *= rndSize;

            // if ((int)Random.Range(0, 10) == 1)
            pig.AddComponent<RandomSqueel>().Innit(pig.gameObject.GetComponent<AudioSource>(), pigSounds, pigSoundWaitingRange, pigSoundVolumeRange); 
        }
    }

    public void SwitchToScreaming() {
        if (switched)
            return;

        switched = true;

        GameObject[] pigs = GameObject.FindGameObjectsWithTag("Pig");
        foreach (GameObject pig in pigs) {
            Animator animator = pig.gameObject.GetComponentInChildren<Animator>();
            if (!animator) { continue; }

            pig.GetComponent<RandomSqueel>().SetSounds(pigScreaming, pigSoundWaitingRange, pigSoundVolumeRange2);
        }
    }
}
