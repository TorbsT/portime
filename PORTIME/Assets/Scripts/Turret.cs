using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform bulletSocket;
    public Transform turretHeadRotator;
    public string actorTag = "Actor";
    public string actorHitboxName = "actorHitbox";
    public float turretRange = 5f;

    GameObject target;  // POTENTIAL CAUSE FOR LAG?
    Transform lastSeen;

    void Start()
    {
        if (bulletSocket == null) Debug.LogError("nullpointerexception");
        if (turretHeadRotator == null) Debug.LogError("nullpointerexception");

        RemoveTarget();
    }
    void Update()
    {
        if (target != null) if (!VerifyVisibility(target.transform))
            {
                Debug.LogWarning("Lost target");
                target = null;
            }
        if (target == null)
        {
            //Debug.Log("Searching");
            target = FindNewTarget();
        }
        if (target != null)
        {
            LookAt(target.transform);
        }
        //else if (lastSeen != null) LookAt(lastSeen);
    }
    void FixedUpdate()
    {

    }
    void RemoveTarget()
    {
        target = null;
        lastSeen = null;
    }
    GameObject FindNewTarget()
    {
        GameObject newTarget = null;
        GameObject[] actors = GameObject.FindGameObjectsWithTag(actorTag);

        foreach (GameObject actor in actors)
        {
            float distance = Vector3.Distance(transform.position, actor.transform.position);
            if (distance < turretRange)
            {
                if (VerifyVisibility(actor.transform))
                {
                    newTarget = actor;
                }
            }
        }
        return newTarget;
    }
    bool VerifyVisibility(Transform victim)
    {
        RaycastHit hit;
        Ray ray = new Ray(turretHeadRotator.position, (victim.position - turretHeadRotator.position));  // rai rai!
        Debug.DrawRay(turretHeadRotator.position, (victim.position - turretHeadRotator.position), Color.red, 0.01f);
        if (Physics.Raycast(ray, out hit, turretRange))
        {
            if (hit.collider.gameObject.name == actorHitboxName)
            {
                return true;
            }
        }
        return false;
    }
    void LookAt(Transform point)
    {
        Quaternion turretSeekLookRotation = Quaternion.LookRotation((point.position-turretHeadRotator.position).normalized);
        turretHeadRotator.rotation = turretSeekLookRotation;
        //Vector3 turretSeekRotation = Quaternion.Lerp(turretHead.rotation, turretSeekLookRotation, Time.deltaTime).eulerAngles;
        //turretHead.rotation = Quaternion.Euler(turretSeekRotation.x, turretSeekRotation.y, turretSeekRotation.z);
    }
}
