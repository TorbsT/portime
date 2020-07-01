using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [System.NonSerialized] public GameObject player;
    public string actorTag = "Actor";

    TimeBody playerTB;

    public bool isRewinding;
    public bool shadowsOnlyInput;
    [SerializeField] float rewindSnappiness = 1f; // does ABSOLUTELY nothing
    [SerializeField] float rewindDuration = 3f;

    public int seq;
    public int globalFrame;  // STATIC, PIVOTED UPON
    public int globalStart;  //
    public List<ActorSequence> sequences;

    public int sequenceFrameLimit = 3*50;

    

    float rewindTime = 0f;
    int globalRewindTo;

    int globalRewindStart;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = GetPlayer();
        globalFrame = 0;
        seq = 0;
        isRewinding = false;
        playerTB = player.GetComponent<TimeBody>();
        sequences = new List<ActorSequence>();
        sequences.Insert(sequences.Count, new ActorSequence(globalStart, new List<BasicFrame>(), new List<ActorFrame>()));
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            //globalFrame--;
            rewindTime += Time.fixedDeltaTime;
            SetCurrentRewindFrame();
            //globalFrame = globalStart;
            if (globalFrame <= globalRewindTo) BackAgain();
        }
        if (!isRewinding)
        {
            globalFrame++;
        }
    }
    public void TravelBack()
    {
        isRewinding = true;
        rewindTime = 0;
        globalRewindStart = globalFrame;
        globalRewindTo = GetNewGlobalStart();
        globalStart = globalRewindTo;


        //newSequenceStart = (Mathf.Max(sequenceStart, sequenceStart + sequenceLength));
        //newSequenceLength = Mathf.Min(sequenceFrameLimit, globalFrame - globalStart);  // 
        //globalSkip = newSequenceSkip;


        Debug.LogWarning(globalStart + " " + globalFrame + " " + globalStart + " " + sequenceFrameLimit);


        // hvis globalframe-globalstart > sequenceframelimit, globalstart = globalframe-sequenceframelimit
        //sequences.Insert(sequences.Count, new Sequence(newSequenceStart, newSequenceLength, newSequenceSkip));
    }

    void BackAgain()
    {
        isRewinding = false;
        playerTB.CreateShadow();
        sequences.Insert(sequences.Count, new ActorSequence(globalStart, new List<BasicFrame>(), new List<ActorFrame>()));
        seq++;
        //
        playerTB.mySeq = seq;
        globalFrame = globalStart;
    }
    public int GetNewGlobalStart()
    {
        return (int)Mathf.Max(globalStart, globalFrame - sequenceFrameLimit);
    }
    void SetCurrentRewindFrame()
    {
        globalFrame = globalRewindStart - GetCurrentRewindFrame();
    }
    int GetCurrentRewindFrame()
    {
        float frame;
        frame = RewindFunction(rewindTime/rewindDuration) / RewindFunction(1);
        if (frame < 0) frame = 0;
        
        frame *=(globalRewindStart-globalRewindTo);
        frame = (Mathf.Floor(frame));
        //if (frame <= 0) Debug.LogWarning("test: " + globalRewindStart + " - " + RewindFunction(rewindTime) + " / " + RewindFunction(rewindDuration) + " * " + (globalRewindStart - GetNewGlobalStart()));
        return (int)frame;
    }
    float RewindFunction(float x)
    {
        float y;
        y = -2f*rewindSnappiness*Mathf.Pow(x, 3f)+3f*rewindSnappiness*Mathf.Pow(x, 2f);
        return y;
    }
    public GameObject GetPlayer()
    {
        GameObject[] actors = GameObject.FindGameObjectsWithTag(actorTag);
        if (actors.Length != 1)
        {
            Debug.LogError("Couldnt find player");
            Debug.LogError(actors.Length);
            Debug.Log(actors[0]);
            Debug.Log(actors[1]);
            return null;
        }
        return actors[0];
    }
}
