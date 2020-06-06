using System.Collections;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    /*
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    private UnityEngine.AI.NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private GameObject player;
    private Animator playerAnim;
    private PlayerHealth playerHealth;
    private HashIDs hash;
    private Vector3 previousSighting;
    private void Awake()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerAnim = player.GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    private void Update()
    {
        if (lastPlayerSighting.position != previousSighting)
            personalLastSighting = lastPlayerSighting.position;

        previousSighting = lastPlayerSighting.position;

        if (playerHealth.health > 0f)
            anim.SetBool(hash.playerInSightBool, playerInSight);
        else
            anim.SetBool(hash.playerInSightBool, false);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fieldOfViewAngle * 0.5f)
            {
                // raycast for colliding
                RaycastHit hit;

                if(Physics.Raycast(transform.position + transform.up * 1f, direction.normalized, out hit, col.radius))
                {
                    if(hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position;
                    }
                }
            }
            // used for sounds, which is not necessary
            //int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
            //int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInSight = false;
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        Vector3 allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for(int i=0; i<path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0f;

        for (int i=0; i<allWayPoints.Length-1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
    */

}
