using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [System.NonSerialized] public GameObject player;
    public string actorTag = "Actor";

    TimeBody playerTB;

    public bool isRewinding;
    //readonly float travelPause = 10f;


    public int seq;
    public int globalFrame;  // STATIC, PIVOTED UPON
    public int globalStart;  //
    public int globalSkip;
    public List<Sequence> sequences;
    public int currentTimeCreatedUpon;
    public int currentSequenceStart;
    public int currentSequenceFrame;
    public int sequenceFrameLimit = 3*50;

    int sequenceStart;
    int sequenceLength;
    int sequenceSkip;
    int newSequenceStart;
    int newSequenceLength;
    int newSequenceSkip;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = GetPlayer();
        globalFrame = 0;
        globalSkip = 0;
        currentSequenceStart = 0;
        seq = 0;
        isRewinding = false;
        playerTB = player.GetComponent<TimeBody>();
        sequences = new List<Sequence>();
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            globalFrame--;
            currentSequenceFrame = globalFrame - currentSequenceStart;
            //globalFrame = globalStart;
            if (globalFrame <= globalStart) BackAgain();
        }
        if (!isRewinding)
        {
            globalFrame++;
            currentSequenceFrame = globalFrame - currentSequenceStart;
        }
    }
    public void TravelBack()
    {
        isRewinding = true;
        playerTB.mySeq = seq;
        Sequence sequence = null;
        if (seq >= 1)
            sequence = sequences[seq - 1];
        if (sequence != null)
        {
            sequenceStart = sequence.start;
            sequenceLength = sequence.length;
            sequenceSkip = sequence.thisSequenceSkip;
        }
        else
        {
            Debug.LogWarning("uh oh STINKYYY");
            sequenceStart = 0;
            sequenceLength = 0;
            sequenceSkip = 0;
        }
        newSequenceStart = (Mathf.Max(sequenceStart, sequenceStart + sequenceLength));
        newSequenceLength = Mathf.Min(sequenceFrameLimit, globalFrame - globalStart);  // 
        //globalSkip = newSequenceSkip;
        globalStart += Mathf.Max(0, globalFrame - globalStart - sequenceFrameLimit);
        newSequenceSkip = globalStart;  // Waits n frames before starting anim, n= deleted frames. Adding each seq = good?

        Debug.LogWarning(globalStart + " " + globalFrame + " " + globalStart + " " + sequenceFrameLimit);


        // hvis globalframe-globalstart > sequenceframelimit, globalstart = globalframe-sequenceframelimit
        sequences.Insert(sequences.Count, new Sequence(newSequenceStart, newSequenceLength, newSequenceSkip));
    }
    void BackAgain()
    {
        isRewinding = false;
        playerTB.CreateShadow();  // BEFORE timeBody.Return()!!!!
        Debug.Log("Start was " + newSequenceStart + ", length was " + newSequenceLength + ", wait was " + newSequenceSkip);
        TimeBody[] timeBodyArray = FindObjectsOfType(typeof(TimeBody)) as TimeBody[];
        foreach (TimeBody timeBody in timeBodyArray)
        {
            //Debug.Log(timeBody.gameObject.name);
            timeBody.Return();
        }
        

        globalFrame = globalStart;
        currentTimeCreatedUpon = newSequenceSkip;
        currentSequenceStart = newSequenceStart + newSequenceLength;
        seq++;
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
