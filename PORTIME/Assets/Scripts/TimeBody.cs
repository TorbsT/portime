using System.Collections;
using System.Collections.Generic;  // NECESSARY FOR LISTS
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public Transform HeadSocket;
    public Transform TorsoSocket;
    public GameObject ActorPrefab;

    public bool isItem = false;
    public bool isActor = false;
    public bool isShadow = false;
    public bool isPlayer = false;
    bool rewindPressed = false;
    bool rPressed = false;

    int framesPlayed;
    int framesStart;
    int framesStop;
    int framesRecorded = 0;
    float pauseTimeFor = 1f;


    public float shadowMouseX;
    public float shadowMouseY;
    public float shadowMoveSide;
    public float shadowMoveForward;
    public bool shadowJump;
    public bool shadowGrab;
    public bool shadowDrop;

    Quaternion actorAllRotation;
    Vector3 lastVelocity = new Vector3(0f, 0f, 0f);

    List<BasicFrame> basicHistory;
    List<ActorFrame> actorHistory;
    Rigidbody rb;
    Actor actor;
    Animator animator;
    SelectionManager selectionManager;


    // Start is called before the first frame update
    void Start()
    {
        if (isPlayer)
            actorHistory = new List<ActorFrame>();
        if (!isShadow)  // Creates empty history for new objects (not shadows, they already have one)
            basicHistory = new List<BasicFrame>();
        else
            framesPlayed = 0;
        rb = GetComponent<Rigidbody>();

        
        if (isActor)
        {
            actor = GetComponent<Actor>();
            selectionManager = GetComponent<SelectionManager>();
            //Head = this.gameObject.transform.GetChild(0);
        }
        if (isShadow)
            rb.isKinematic = true;
            animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            rewindPressed = true;
        }
    }

    void FixedUpdate()
    {
        pauseTimeFor -= Time.fixedDeltaTime;
        if (pauseTimeFor > 0)
        {
            rb.isKinematic = true;
            return;
        }
        else if (pauseTimeFor > -1 && !isShadow)
            rb.isKinematic = false;

        if (rewindPressed)
        {
            if (isActor)
            {
                //GetComponent<SelectionManager>().ClearGrab();
            }
            rewindPressed = false;
            if (isPlayer)
            {
                CreateShadow();
            }
            else if (isShadow)
            {
                framesPlayed = 0;
            }
            else  // Objects remove their history and resets to initial
            {
                Restart();
            }
        }

        if (isPlayer)
        {
            actor.UpdateAsPlayer();
            Record();
        }
        else if (isShadow)
        {
            Replay();
        }
        else
            Record();

        
    }
    void Replay()
    {
        if (framesPlayed == 0)
            ReplayFrame(framesStart);
        else if (isShadow) //framesPlayed < framesStop-framesStart)  
        {
            ReplayFrame(framesPlayed + framesStart);
            //ReplayFrameUsingInput(framesPlayed+framesStart);
        } else
        {
            // STORE AWAY FOR LATER USE
            transform.position = new Vector3(0f, 0f, 0f);
        }
        framesPlayed++;
    }
    void Restart()
    {
        ReplayFrame(0);
        basicHistory.RemoveRange(1, basicHistory.Count-1);  // First frame is necessary for future restarts
    }
    void ReplayFrameUsingInput(int index)
    {
        ActorFrame actorFrame = actorHistory[index];
        shadowMouseX = actorFrame.mouseX;
        shadowMouseY = actorFrame.mouseY;
        shadowMoveSide = actorFrame.moveSide;
        shadowMoveForward = actorFrame.moveForward;
        shadowJump = actorFrame.jump;
        shadowGrab = actorFrame.grab;
        shadowDrop = actorFrame.drop;

        actor.UpdateAsShadow();
    }
    void ReplayFrame(int index)
    {
        BasicFrame basicFrame = basicHistory[index];
        transform.position = basicFrame.position;
        transform.rotation = basicFrame.rotation;
        rb.velocity = basicFrame.velocity;
        rb.angularVelocity = basicFrame.angularVelocity;

        if (isActor)
        {
            ActorFrame actorFrame = actorHistory[index];
            TorsoSocket.transform.rotation = actorFrame.torsoRotation;
            HeadSocket.transform.rotation = actorFrame.headRotation;
            shadowGrab = actorFrame.grab;
            shadowDrop = actorFrame.drop;
            selectionManager.ObjectInteraction(shadowGrab, shadowDrop);
            animator.SetFloat("Velocity", basicFrame.velocity.sqrMagnitude);
        }

    }
    void Record()
    {
        // ALL time objects record basic history, just in case.
        if (basicHistory.Count > Mathf.Round(60f / Time.fixedDeltaTime))
        {
            TimeUp();
            Debug.LogError("bruh");
            basicHistory.RemoveAt(0);
        }
        basicHistory.Insert(basicHistory.Count, new BasicFrame(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));

        // actors record input & extra rotation as well.
        if (isActor)
        {
            if (actorHistory.Count > Mathf.Round(60f / Time.fixedDeltaTime))
            {
                TimeUp();
                Debug.LogError("bruh");
                actorHistory.RemoveAt(0);
            }
            //actorAllRotation = Head.transform.rotation;
            actorHistory.Insert(actorHistory.Count, new ActorFrame(TorsoSocket.rotation, HeadSocket.rotation, actor.mouseX, actor.mouseY, actor.moveSide, actor.moveForward, actor.jump, actor.grab, actor.drop));

        }
        
        framesRecorded++;
    }
    void TimeUp()
    {
        Debug.Log("basicHistory: " + basicHistory.Count + ", actorHistory: " + actorHistory.Count);
    }

    void CreateShadow()
    {
        GameObject Shadow = Instantiate(ActorPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        TimeBody ShadowProperties = Shadow.GetComponent<TimeBody>();
        ShadowProperties.isShadow = true;
        ShadowProperties.isPlayer = false;
        ShadowProperties.framesStart = basicHistory.Count-framesRecorded;
        ShadowProperties.framesStop = basicHistory.Count;
        //ShadowProperties.pauseTimeFor = 0f;
        ShadowProperties.basicHistory = basicHistory;
        ShadowProperties.actorHistory = actorHistory;

        framesRecorded = 0;
    }


}
