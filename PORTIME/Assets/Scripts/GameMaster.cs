using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    TimeBody playerTB;

    public float timeLeft;
    public int framesPlayed;
    public float pauseTimeFor;

    readonly float totalTime = 120f;
    int globalFramesStart;
    readonly float travelPause = 1f;
    public readonly float sequenceLimit = 10f;

    private void Awake()
    {
        globalFramesStart = 0;
        timeLeft = totalTime;
        pauseTimeFor = travelPause;
        playerTB = GameObject.Find("Player").GetComponent<TimeBody>();
    }
    private void FixedUpdate()
    {
        timeLeft -= Time.fixedDeltaTime;
        pauseTimeFor -= Time.fixedDeltaTime;
        if (pauseTimeFor <= 0)
            framesPlayed++;
    }
    public void TravelBack()
    {
        globalFramesStart += playerTB.deletedFrames;
        framesPlayed = globalFramesStart;
        pauseTimeFor = travelPause;
        Debug.Log("global frames start: " + globalFramesStart);
        TimeBody[] timeBodyArray = FindObjectsOfType(typeof(TimeBody)) as TimeBody[];
        foreach (TimeBody timeBody in timeBodyArray)
        {
            timeBody.Return();
        }
    }
}
