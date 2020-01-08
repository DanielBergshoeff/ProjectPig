using UnityEngine;

public class SetupPigAnimations : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] animators;

    private void Start()
    {
        GameObject[] pigs = GameObject.FindGameObjectsWithTag("Pig");
        foreach (GameObject pig in pigs)
        {
            Animator animator = pig.gameObject.GetComponent<Animator>();
            animator.runtimeAnimatorController = animators[Random.Range(0, animators.Length)];
            float rndSize = Random.Range(0.5f, 1.5f);
            animator.SetFloat("Speed", rndSize);

            rndSize = Random.Range(0.75f, 1f);
            pig.transform.localScale *= rndSize;

            rndSize = Random.Range(0.25f, 1.25f);
            Material mat = pig.GetComponentInChildren<Renderer>().material;
            mat.color *= rndSize;
        }
    }
}
