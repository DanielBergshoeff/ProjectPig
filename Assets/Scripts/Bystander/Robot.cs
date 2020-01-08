using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform pathParent;
    [SerializeField] private float moveSpeed = 1.0f;

    [Header("Find player")]
    [SerializeField] private float viewDistance = 20.0f;
    [SerializeField] private float viewAngle = 50.0f;
    [SerializeField] private float timeTillLost = 5.0f;
    [SerializeField] private float attackDistance = 2.0f;
    [SerializeField] private float chasingSpeed = 2.0f;

    [Header("Attributes")]
    [SerializeField] private Light mySpotLight;
    [SerializeField] public bool evil = true;
    [SerializeField] private Color evilLightColor;
    [SerializeField] private Color goodLightColor;

    private Transform[] path;
    private int currentPathPosition = 0;
    private NavMeshAgent myNavMeshAgent;
    private bool playerSpotted = false;
    private bool playerLost = false;
    private float timeLost = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();

        path = new Transform[pathParent.childCount];
        for (int i = 0; i < pathParent.childCount; i++)
        {
            path[i] = pathParent.GetChild(i);
        }

        myNavMeshAgent.speed = moveSpeed;

        SetEvil();
    }

    // Update is called once per frame
    void Update()
    {
        SetEvil();

        Move();
        /*if (evil)
            CheckForPlayer();*/
    }

    public void SetEvil()
    {
        if (evil)
            mySpotLight.color = evilLightColor;
        else
            mySpotLight.color = goodLightColor;
    }


    private void Move()
    {
        if (!playerSpotted)
            CheckForPathUpdate();
        else
        {
            WalkToPlayer();

            if (playerLost)
            {
                timeLost += Time.deltaTime;
                if (timeLost > timeTillLost)
                {
                    playerSpotted = false;
                    myNavMeshAgent.speed = moveSpeed;
                    myNavMeshAgent.SetDestination(path[currentPathPosition].position);
                }
            }
        }
    }

    private void CheckForPathUpdate()
    {
        if (Vector3.Distance(transform.position, path[currentPathPosition].position) < 0.1f)
        {
            if (currentPathPosition < path.Length - 1)
            {
                currentPathPosition++;
            }
            else
            {
                currentPathPosition = 0;
                ReturnToStart();
            }
            myNavMeshAgent.speed = moveSpeed;
            myNavMeshAgent.SetDestination(path[currentPathPosition].position);
        }
    }

    private void WalkToPlayer()
    {
        myNavMeshAgent.speed = chasingSpeed;
        myNavMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    }

    private void CheckForPlayer()
    {
        Vector3 heading = GameManager.Instance.Player.transform.position - transform.position;
        playerLost = true;

        if (heading.magnitude > viewDistance)
            return;

        if (Mathf.Abs(Vector3.Angle(transform.forward, heading.normalized)) > viewAngle)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, heading.normalized, out hit, viewDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                playerSpotted = true;
                playerLost = false;
                timeLost = 0f;

                if (heading.magnitude < attackDistance)
                {
                    GameManager.Instance.Respawn();
                }
            }
        }
    }

    private void ReturnToStart()
    {
        transform.position = path[0].position;
    }
}
