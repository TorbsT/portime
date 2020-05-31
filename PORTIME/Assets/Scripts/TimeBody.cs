using System.Collections;
using System.Collections.Generic;  // NECESSARY FOR LISTS
using UnityEngine;

public class TimeBody : MonoBehaviour
{

    public Transform ActorAllRotator;
    public GameObject ActorPrefab;

    public bool isReplaying = false;
    public bool isActor = false;
    public bool isShadow = false;
    public bool isPlayer = false;


    int framesPlayed;
    int framesStart;
    int framesStop;

    int framesRecorded = 0;

    Quaternion actorAllRotation;
    Vector3 lastVelocity = new Vector3(0f, 0f, 0f);

    List<PointInTime> pointsInTime;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        
        if (!isShadow)
            pointsInTime = new List<PointInTime>();
        else
            framesPlayed = 0;
        rb = GetComponent<Rigidbody>();
        if (isActor)
        {
            ActorAllRotator = this.gameObject.transform.GetChild(0);
        }
        else
            ActorAllRotator = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e")  )
        {
            if (isPlayer)
            {
                CreateShadow();
            } else if (isShadow)
            {
                framesPlayed = 0;
            }
            else
            {
                Restart();
            }
        }
    }

    void FixedUpdate()
    {
        if (isShadow)
        {
            Replay();
        }
        else
            Record();
    }
    void Replay()
    {
        if (isShadow)
        {
            Debug.Log("frame nr. " + framesPlayed + ", framesStart: " + framesStart + ", framesStop: " + framesStop + ", lengde: " + pointsInTime.Count);
            if (framesPlayed < framesStop-framesStart)
            {
                ReplayFrame(framesPlayed+framesStart);
                //if (isShadow && pointsInTime.Count > 1)
                //    pointsInTime.RemoveAt(pointsInTime.Count - 1);
            } else
            {
                Debug.Log("Æ DØR");
                //framesPlayed = 0;
                // TODO: Gjør at shadows har deep copy av history
                // JK fant en anna metode
                //Object.Destroy(this.gameObject);
                transform.position = new Vector3(0f, 0f, 0f);
            }
            framesPlayed++;
        }
    }
    void DeepCopy()
    {

    }
    void Restart()
    {
        ReplayFrame(0);
        pointsInTime.RemoveRange(1, pointsInTime.Count-1);  // Removes everything except first frame
    }
    void ReplayFrame(int index)
    {
        PointInTime pointInTime = pointsInTime[index];
        transform.position = pointInTime.position;
        transform.rotation = pointInTime.rotation;
        rb.velocity = pointInTime.velocity;
        if (isActor)
        {
            ActorAllRotator.transform.rotation = pointInTime.actorAllRotation;
        }
    }
    void Record()
    {
        if (pointsInTime.Count > Mathf.Round(60f / Time.fixedDeltaTime))
        {
            Debug.LogError("bruh");
            pointsInTime.RemoveAt(0);
        }

        if (isActor)
            actorAllRotation = ActorAllRotator.transform.rotation;
        else
            actorAllRotation = Quaternion.Euler(0f, 0f, 0f);
        pointsInTime.Insert(pointsInTime.Count, new PointInTime(transform.position, transform.rotation, rb.velocity, actorAllRotation));
        framesRecorded++;
    }

    void CreateShadow()
    {
        GameObject Shadow = Instantiate(ActorPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        TimeBody ShadowProperties = Shadow.GetComponent<TimeBody>();
        ShadowProperties.isShadow = true;
        ShadowProperties.isPlayer = false;
        ShadowProperties.framesStart = pointsInTime.Count-framesRecorded;
        ShadowProperties.framesStop = pointsInTime.Count;
        ShadowProperties.pointsInTime = pointsInTime;

        framesRecorded = 0;
    }


}
