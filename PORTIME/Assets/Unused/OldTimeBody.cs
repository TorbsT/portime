using System.Collections;
using System.Collections.Generic;  // NECESSARY FOR LISTS
using UnityEngine;

public class OldTimeBody : MonoBehaviour
{
    /*
    public Transform ActorAllRotator;

    public static bool isRewinding = false;
    bool isActor = false;
    bool isShadow = false;
    Quaternion actorAllRotation;
    Vector3 lastVelocity = new Vector3(0f, 0f, 0f);

    List<PointInTime> pointsInTime;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
        if (transform.childCount > 0)
        {
            isActor = true;
            ActorAllRotator = this.gameObject.transform.GetChild(0);
        }
        else
            ActorAllRotator = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShadow)
        {
            Debug.Log("Shadow");
        }
        else
        {


            if (!isRewinding && Input.GetKey("e"))
                StartRewind();
            if (isRewinding && !Input.GetKey("e"))
                StopRewind();

        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind(0);
        else
            Record();
    }

    void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[index];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            lastVelocity = pointInTime.velocity;
            if (isActor)
            {
                ActorAllRotator.transform.rotation = pointInTime.actorAllRotation;
            }
            Debug.Log("In log: " + pointInTime.velocity + ", now: " + rb.velocity);
            if (pointsInTime.Count > 1)
                pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }

    }
    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(5f / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        if (isActor)
            actorAllRotation = ActorAllRotator.transform.rotation;
        else
            actorAllRotation = Quaternion.Euler(0f, 0f, 0f);
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.velocity, actorAllRotation));
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        rb.velocity = lastVelocity;
    }
    */
}
