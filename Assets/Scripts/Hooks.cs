using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooks : MonoBehaviour
{
    public List<Pig> Pigs;
    public List<GameObject> AllHooks;


    [Header("Pig variables")]
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private Transform pigSpawnPosition;

    [Header("Keycodes")]
    [SerializeField] private KeyCode keySpawn = KeyCode.Alpha1;
    [SerializeField] private CollisionObject checkPigTrigger;

    [Header("Light")]
    [SerializeField] private Light checkLight;
    [SerializeField] private Color correctLightColor;
    [SerializeField] private Color wrongLightColor;

    private bool newPigAllowed = true;
    private Animator myAnimator;
    private Vector3[] pigPositions;
    private bool pigDropTime = false;
    // Start is called before the first frame update
    void Start()
    {
        pigPositions = new Vector3[Pigs.Count];
        myAnimator = GetComponent<Animator>();

        if (checkPigTrigger.triggerEvent == null)
            checkPigTrigger.triggerEvent = new TriggerEvent();

        checkPigTrigger.triggerEvent.AddListener(CheckPig);
    }

    // Update is called once per frame
    void Update()
    {
        if (pigDropTime) {
            DropPig();
            pigDropTime = false;
        }

        if (Input.GetKeyDown(keySpawn) && newPigAllowed) {
            myAnimator.SetTrigger("Move");
            newPigAllowed = false;
        }
    }

    public void SetPigDrop() {
        pigDropTime = true;
        PlayerDrownController.Instance.ResetDrown();

        Pigs[Pigs.Count - 1].transform.parent = null;
        Pigs[Pigs.Count - 1].UnHook();

        for (int i = AllHooks.Count - 1; i > 0; i--) {
            Pigs[i] = Pigs[i - 1];
            Pigs[i].transform.SetParent(null);
        }
    }

    public void DropPig() {
        for (int i = AllHooks.Count - 1; i > 0; i--) {
            Pigs[i].transform.SetParent(AllHooks[i].transform);
        }

        SpawnPig();
    }

    /// <summary>
    /// Instantiate pig at the pig spawn position
    /// </summary>
    private void SpawnPig() {
        GameObject pig = Instantiate(pigPrefab, pigSpawnPosition.transform.position, pigSpawnPosition.transform.rotation);
        Pigs[0] = pig.GetComponent<Pig>();
        Pigs[0].transform.SetParent(AllHooks[0].transform);
    }

    public void CheckPig(Collider coll, bool enter) {
        if (!enter)
            return;

        Pig pig = coll.gameObject.GetComponentInParent<Pig>();
        if (pig == null)
            return;

        if (pig.Checked)
            return;

        pig.Checked = true;
        newPigAllowed = true;

        if (pig.Dehaired)
            checkLight.color = correctLightColor;
        else
            checkLight.color = wrongLightColor;
    }
}
