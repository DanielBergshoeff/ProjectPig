using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hooks : MonoBehaviour
{
    public List<DehairingPig> Pigs;
    public List<GameObject> AllHooks;

    public static Hooks Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI processTime;
    [SerializeField] private TextMeshProUGUI shortestProcessTime;
    private float processTimer = 0f;
    private float shortestProcessTimer = 100f;
    [SerializeField] private int correctPigsTillTimerGone;
    [SerializeField] private CollisionObject resetObject;

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
    [SerializeField] private MeshRenderer robotHeadMesh;

    [Header("Audio")]
    [SerializeField] private AudioSource reactionSound;
    [SerializeField] private AudioClip positiveClip;
    [SerializeField] private AudioClip negativeClip;

    [SerializeField] private AudioSource heartBeatAudioSource;
    [SerializeField] private float heartBeatSpeed = 3.0f;

    [SerializeField] private GameObject transitionObject;
    [SerializeField] private DialogueObject dialoguePreVictim;

    private bool newPigAllowed = true;
    private Animator myAnimator;
    private Vector3[] pigPositions;
    private bool pigDropTime = false;
    private bool pigProcess = false;
    private bool livingPig = false;

    private int amtOfCorrectPigs = 0;

    private AudioSource myAudioSource;
    private SceneLoader sceneLoader;

    private void Start()
    {
        Instance = this;

        pigPositions = new Vector3[Pigs.Count];
        myAnimator = GetComponent<Animator>();

        if (checkPigTrigger.triggerEvent == null)
            checkPigTrigger.triggerEvent = new TriggerEvent();

        if (resetObject.collisionEvent == null)
            resetObject.collisionEvent = new CollisionEvent();

        checkPigTrigger.triggerEvent.AddListener(CheckPig);
        resetObject.collisionEvent.AddListener(ResetPig);

        myAudioSource = GetComponent<AudioSource>();
        sceneLoader = gameObject.AddComponent<SceneLoader>();

        checkLight.color = wrongLightColor;
        robotHeadMesh.materials[5].SetColor("_EmissionColor", wrongLightColor);
        robotHeadMesh.materials[1].SetColor("_EmissionColor", wrongLightColor);
    }

    private void Update()
    {
        if (pigDropTime)
        {
            DropPig();
            pigDropTime = false;
            pigProcess = true;
        }

        if (Input.GetKeyDown(keySpawn) && newPigAllowed)
        {
            SetupNextPig();
        }

        if (pigProcess)
        {
            processTimer += Time.deltaTime;
            processTime.text = processTimer.ToString("F2");
        }
    }

    public void SetupNextPig()
    {
        myAudioSource.Play();
        myAnimator.SetTrigger("Move");
        newPigAllowed = false;
    }

    public void SetPigDrop()
    {
        pigDropTime = true;
        PlayerDrownController.Instance.ResetDrown();

        Pigs[Pigs.Count - 1].transform.parent = null;
        Pigs[Pigs.Count - 1].UnHook();

        processTimer = 0f;
        processTime.text = processTimer.ToString("F2");

        for (int i = AllHooks.Count - 1; i > 0; i--)
        {
            Pigs[i] = Pigs[i - 1];
            Pigs[i].transform.SetParent(null);
        }
    }

    public void DropPig()
    {
        for (int i = AllHooks.Count - 1; i > 0; i--)
        {
            Pigs[i].transform.SetParent(AllHooks[i].transform);
        }

        SpawnPig();
    }

    /// <summary>
    /// Instantiate pig at the pig spawn position
    /// </summary>
    private void SpawnPig()
    {
        GameObject pig = Instantiate(pigPrefab, pigSpawnPosition.transform.position, pigSpawnPosition.transform.rotation);
        Pigs[0] = pig.GetComponent<DehairingPig>();
        Pigs[0].transform.SetParent(AllHooks[0].transform);

        if (!livingPig)
            return;

        livingPig = false;
        pig.GetComponent<HalfDeadPig>().enabled = true;
        heartBeatAudioSource.pitch = heartBeatSpeed;
        heartBeatAudioSource.Play();
        Pigs[0].Alive = true;
    }

    private void ResetPig(Collision coll, bool enter)
    {
        if (!enter)
            return;

        DehairingPig pig = coll.gameObject.GetComponentInParent<DehairingPig>();
        if (pig == null)
            return;

        if (!pig.Checked)
        {
            pig.Checked = true;
            newPigAllowed = true;
            pigProcess = false;
        }

        Destroy(pig.gameObject);
    }

    public void UpdateHeartBeat(float drowning) {
        heartBeatAudioSource.pitch = drowning * heartBeatSpeed;
    }

    public void CheckPig(Collider coll, bool enter) {
        if (!enter)
            return;

        DehairingPig pig = coll.gameObject.GetComponentInParent<DehairingPig>();
        if (pig == null)
            return;

        if (pig.Checked)
            return;

        pig.Checked = true;
        newPigAllowed = true;
        pigProcess = false;

        if (pig.Dehaired && pig.GetComponent<HalfDeadPig>().enabled) {
            sceneLoader.LoadScene("PreVictim", transitionObject, dialoguePreVictim);
        }
        else if(pig.GetComponent<HalfDeadPig>().enabled) {
            livingPig = true;
        }
        
        if (pig.Dehaired) {
            if (processTimer < shortestProcessTimer) {
                shortestProcessTimer = processTimer;
                shortestProcessTime.text = shortestProcessTimer.ToString("F2");
            }

            checkLight.color = correctLightColor;
            robotHeadMesh.materials[5].SetColor("_EmissionColor", correctLightColor);
            robotHeadMesh.materials[1].SetColor("_EmissionColor", correctLightColor);

            amtOfCorrectPigs++;
            if (amtOfCorrectPigs == correctPigsTillTimerGone)
            {
                //PlayerDrownController.Instance.DisableTimer();
                livingPig = true;
            }
            reactionSound.PlayOneShot(positiveClip);
        }
        else
        {
            checkLight.color = wrongLightColor;
            robotHeadMesh.materials[5].SetColor("_EmissionColor", wrongLightColor);
            robotHeadMesh.materials[1].SetColor("_EmissionColor", wrongLightColor);

            amtOfCorrectPigs = 0;
            reactionSound.PlayOneShot(negativeClip);
        }
    }
}
