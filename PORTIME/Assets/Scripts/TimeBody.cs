using System.Collections;
using System.Collections.Generic;  // NECESSARY FOR LISTS
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public Transform HeadSocket;
    public Transform TorsoSocket;
    public Transform blockRotator;
    public GameObject ActorPrefab;

    public bool isBlock = false;
    public bool isActor = false;
    public bool isShadow = false;
    public bool isPlayer = false;

    int framesStart;
    int framesStop;
    int framesRecorded = 0;

    List<BasicFrame> basicHistory;
    List<ActorFrame> actorHistory;
    Rigidbody rb;
    Actor actor;
    Animator animator;
    SelectionManager selectionManager;
    GameMaster gameMaster;

    // Start is called before the first frame update
    void Awake()
    {
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }
    void Start()
    {
        if (isPlayer)
            actorHistory = new List<ActorFrame>();
        if (!isShadow)  // Creates empty history for new objects (not shadows, they already have one)
            basicHistory = new List<BasicFrame>();
        //else
        //    framesPlayed = 0;
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
    public void Return()
    {
        if (isActor)
        {
            selectionManager.Drop();
        }
        if (isPlayer)
        {
            Debug.Log("bruh");
            transform.position = basicHistory[0].position;
            CreateShadow();
        }
        //else if (isShadow)
        //{
        //   framesPlayed = 0;
        //}
        else if (!isShadow)  // Objects remove their history and resets to initial
        {
            Restart();
        }
    }

    void FixedUpdate()
    {
        if (gameMaster.pauseTimeFor > 0)
        {
            rb.isKinematic = true;
            return;
        }
        else if (gameMaster.pauseTimeFor > -1 && !isShadow)
            rb.isKinematic = false;

        if (isPlayer)
        {
            actor.UpdateActor();
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
        if (gameMaster.framesPlayed == 0)
            ReplayFrame(framesStart);
        else if (isShadow && gameMaster.framesPlayed < framesStop - framesStart) //)  
        {
            Debug.Log("gameMaster.framesPlayed: " + gameMaster.framesPlayed + ", framesStart: " + framesStart + ", object: " + gameObject.name);
            ReplayFrame(gameMaster.framesPlayed + framesStart);
            //ReplayFrameUsingInput(framesPlayed+framesStart);
        } else
        {
            // STORE AWAY FOR LATER USE
            //transform.position = new Vector3(0f, -100f, 0f);
        }
        //framesPlayed++;
    }
    void Restart()
    {
        ReplayFrame(0);
        basicHistory.RemoveRange(1, basicHistory.Count-1);  // First frame is necessary for future restarts
    }
    void ReplayFrame(int index)
    {
        Debug.Log(index + ", " + basicHistory.Count);
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
            blockRotator.transform.rotation = actorFrame.blockRotation;
            selectionManager.ObjectInteraction(actorFrame.grab, actorFrame.drop);
            actor.UpdateShadow(actorFrame);
            
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
            actorHistory.Insert(actorHistory.Count, new ActorFrame(TorsoSocket.rotation, HeadSocket.rotation, blockRotator.rotation, actor.mouseX, actor.mouseY, actor.moveSide, actor.moveForward, actor.jump, actor.grab, actor.drop, actor.rotate, actor.shift));

        }
        framesRecorded++;
    }
    void TimeUp()
    {
        Debug.Log("basicHistory: " + basicHistory.Count + ", actorHistory: " + actorHistory.Count);
    }

    void CreateShadow()
    {
        GameObject Shadow = Instantiate(ActorPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
        TimeBody ShadowProperties = Shadow.GetComponent<TimeBody>();
        ShadowProperties.isShadow = true;
        ShadowProperties.isPlayer = false;
        ShadowProperties.framesStart = basicHistory.Count-framesRecorded;
        Debug.LogWarning("count: " + basicHistory.Count + ", recorded: " + framesRecorded);
        ShadowProperties.framesStop = basicHistory.Count;
        //ShadowProperties.pauseTimeFor = 0f;
        ShadowProperties.basicHistory = basicHistory;
        ShadowProperties.actorHistory = actorHistory;

        framesRecorded = 0;
    }


}
