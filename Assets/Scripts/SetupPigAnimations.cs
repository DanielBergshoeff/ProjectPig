using System.Linq;
using UnityEngine;

public class SetupPigAnimations : MonoBehaviour
{
    [SerializeField] private Vector2 animationSpeedRange = new Vector2(.5f, 1.5f);
    [SerializeField] private Vector2 colorTintRange = new Vector2(.75f, 1f);
    [SerializeField] private Vector2 sizeRange = new Vector2(.25f, 1.25f);
    private RuntimeAnimatorController[] animators;

    private void Start()
    {
        animators = Resources.LoadAll("PigAnimationControllers", typeof(RuntimeAnimatorController)).Cast<RuntimeAnimatorController>().ToArray();

        GameObject[] pigs = GameObject.FindGameObjectsWithTag("Pig");
        foreach (GameObject pig in pigs)
        {
            Animator animator = pig.gameObject.GetComponent<Animator>();
            if (!animator) { continue; }
            animator.runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
            float rndSize = Random.Range(animationSpeedRange.x, animationSpeedRange.y);
            animator.SetFloat("Speed", rndSize);

            rndSize = Random.Range(colorTintRange.x, colorTintRange.y);
            pig.transform.localScale *= rndSize;

            rndSize = Random.Range(sizeRange.x, sizeRange.y);
            Material mat = pig.GetComponentInChildren<Renderer>().material;
            mat.color *= rndSize;
        }
    }
}
