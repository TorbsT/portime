using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    TimeBody playerTB;

    public float pauseTimeFor;
    readonly float travelPause = 1f;


    public int seq;
    public int globalFrame;  // STATIC, PIVOTED UPON
    public int globalStart;  //
    public List<Sequence> sequences;
    public int currentTimeCreatedUpon;
    public int currentSequenceStart;
    public int currentSequenceFrame;
    public int sequenceFrameLimit = 50*5;

    private void Awake()
    {
        globalFrame = 0;
        currentSequenceStart = 0;
        seq = 0;
        pauseTimeFor = travelPause;
        playerTB = GameObject.Find("Player").GetComponent<TimeBody>();
        sequences = new List<Sequence>();
    }
    private void FixedUpdate()
    {
        pauseTimeFor -= Time.fixedDeltaTime;
        if (pauseTimeFor <= 0)
        {
            globalFrame++;
            currentSequenceFrame = globalFrame - currentSequenceStart;
        }
    }
    public void TravelBack()
    {
        Sequence sequence = null;
        if (seq >= 1)
            sequence = sequences[seq-1];
        int sequenceStart;
        int sequenceLength;
        int sequenceSkip;
        if (sequence != null)
        {
            sequenceStart = sequence.start;
            sequenceLength = sequence.length;
            sequenceSkip = sequence.skip;
        } else
        {
            Debug.LogWarning("uh oh STINKYYY");
            sequenceStart = 0;
            sequenceLength = 0;
            sequenceSkip = 0;
        }
        int newSequenceStart = (Mathf.Max(sequenceStart, sequenceStart + sequenceLength));
        int newSequenceLength = Mathf.Min(sequenceFrameLimit, globalFrame - globalStart);  // 
        int newSequenceSkip = Mathf.Max(0, globalFrame - globalStart - sequenceFrameLimit) + sequenceSkip;  // Waits n frames before starting anim, n= deleted frames. Adding each seq = good?

        Debug.LogWarning(globalStart + " " + globalFrame + " " + globalStart + " " + sequenceFrameLimit);
        globalStart = Mathf.Max(globalStart, globalFrame - globalStart - sequenceFrameLimit);
        sequences.Insert(sequences.Count, new Sequence(newSequenceStart, newSequenceLength, newSequenceSkip));

        Debug.Log("Start was " + newSequenceStart + ", length was " + newSequenceLength + ", wait was " + newSequenceSkip);
        TimeBody[] timeBodyArray = FindObjectsOfType(typeof(TimeBody)) as TimeBody[];
        foreach (TimeBody timeBody in timeBodyArray)
        {
            timeBody.Return();
        }

        globalFrame = globalStart;
        currentTimeCreatedUpon = newSequenceSkip;
        currentSequenceStart = newSequenceStart + newSequenceLength;
        seq++;
    }
}
