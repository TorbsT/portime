using System.Collections;
using System.Collections.Generic;  // NECESSARY FOR LISTS
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public Transform HeadSocket;
    public Transform TorsoSocket;
    public Transform blockRotator;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] GameObject ghostPrefab;

    public bool isBlock = false;
    public bool isActor = false;
    public bool isShadow = false;
    public bool isPlayer = false;
    public bool isGhost = false;

    public int mySeq;
    Sequence sequence;

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
        if (isPlayer)
        {
            actorHistory = new List<ActorFrame>();
        }
        if (!isShadow)  // Creates empty history for new objects (not shadows, they already have one)
            basicHistory = new List<BasicFrame>();
    }
    void Start()
    {

        //else
        //    framesPlayed = 0;
        rb = GetComponent<Rigidbody>();
        /*
        if (isShadow)
        {
            sequence = gameMaster.sequences[mySeq];
        }
        */
        if (isActor)
        {
            actor = GetComponent<Actor>();
            selectionManager = GetComponent<SelectionManager>();
            //Head = this.gameObject.transform.GetChild(0);
        }
        if (isShadow && rb != null)
            rb.isKinematic = true;
            animator = GetComponent<Animator>();
        if (isPlayer)
        {
            //CreateGhost();
        }

    }

    void FixedUpdate()
    {
        if (gameMaster.pauseTimeFor > 0 && rb != null)
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
        sequence = gameMaster.sequences[mySeq];
        int currentRelativeFrame = gameMaster.globalFrame-gameMaster.sequences[mySeq].thisSequenceSkip;  // When at 0, play. when at sequence.length, stop
        if (currentRelativeFrame >= 0 && currentRelativeFrame < sequence.length) //)  
        {
            if (mySeq == 1) Debug.Log("currentRelativeFrame: " + currentRelativeFrame + ", " + gameMaster.globalFrame + " " + gameMaster.globalStart + " " + gameMaster.globalSkip);
            //Debug.Log("currentRelativeFrame: " + currentRelativeFrame + ", framesStart: " + framesStart + ", framesStop: " + framesStop);
            ReplayFrame(currentRelativeFrame + sequence.start);
        } else
        {
            if (mySeq == 1) Debug.LogWarning("currentRelativeFrame: " + currentRelativeFrame + ", " + sequence.length + " " + gameMaster.globalFrame + " " + gameMaster.globalStart + " " + gameMaster.globalSkip);
            // STORE AWAY FOR LATER USE
            transform.position = new Vector3(0f, -100f, 0f);
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
        //Debug.Log(index + ", " + basicHistory.Count);
        if (index < 0 ^ index >= basicHistory.Count) Debug.LogWarning("error, " + index + ", " + basicHistory.Count);
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
        // ALL time objects record basic history, just in case
        if (isActor)
        {
            // actors record input & extra rotation as well.
            if (gameMaster.globalFrame-gameMaster.globalStart > gameMaster.sequenceFrameLimit)
            {
                //TimeUp();
                //Debug.LogError("bruh");
                basicHistory.RemoveAt(gameMaster.currentSequenceStart);
                actorHistory.RemoveAt(gameMaster.currentSequenceStart);
                Debug.Log("Removed at " + gameMaster.currentSequenceStart);
            }
            //actorAllRotation = Head.transform.rotation;
            basicHistory.Insert(basicHistory.Count, new BasicFrame(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
            actorHistory.Insert(actorHistory.Count, new ActorFrame(TorsoSocket.rotation, HeadSocket.rotation, blockRotator.rotation, actor.mouseX, actor.mouseY, actor.moveSide, actor.moveForward, actor.jump, actor.grab, actor.drop, actor.rotate, actor.shift));
            //Debug.Log(basicHistory.Count);
        }
        else
        {
            if (gameMaster.globalFrame > 3*gameMaster.sequenceFrameLimit)
            {
                //TimeUp();
                //Debug.LogError("bruh");
                basicHistory.RemoveAt(0);
            }
            basicHistory.Insert(basicHistory.Count, new BasicFrame(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
        }
    }
    void TimeUp()
    {
        Debug.Log("basicHistory: " + basicHistory.Count + ", actorHistory: " + actorHistory.Count);
    }

    void CreateShadow()
    {
        GameObject Shadow = Instantiate(shadowPrefab, new Vector3(0f, -100f, 0f), Quaternion.identity);
        TimeBody ShadowProperties = Shadow.GetComponent<TimeBody>();
        ShadowProperties.isShadow = true;
        ShadowProperties.isPlayer = false;
        
        ShadowProperties.mySeq = gameMaster.seq;
        ShadowProperties.basicHistory = basicHistory;
        ShadowProperties.actorHistory = actorHistory;
    }
    /*
    void CreateGhost()
    {
        GameObject ghost = Instantiate(ghostPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        TimeBody ghostTB = ghost.GetComponent<TimeBody>();
        ghostTB.isShadow = true;
        ghostTB.isPlayer = false;
        ghostTB.isGhost = true;

        ghostTB.basicHistory = basicHistory;
        Debug.Log(basicHistory);
        ghostTB.actorHistory = actorHistory;
        ghostTB.framesStop = 1000000000;
        ghostTB.framesStart = framesStart-1;
        ghostTB.deletedFrames = (int)Mathf.Round(gameMaster.sequenceLimit * Time.fixedDeltaTime);
    }
    */
    public void Return()
    {
        if (isActor)
        {
            selectionManager.Drop();
        }
        if (isPlayer)
        {
            Debug.Log("bruh");
            ReplayFrame(gameMaster.currentSequenceStart);  // when time is paused, set position and shit to the first frame
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


}
