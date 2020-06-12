using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float timeLeft;
    public int framesPlayed;
    public float pauseTimeFor;

    readonly float totalTime = 120f;
    readonly float travelPause = 1f;

    private void Awake()
    {
        timeLeft = totalTime;
        pauseTimeFor = travelPause;
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
        framesPlayed = 0;
        pauseTimeFor = travelPause;
        TimeBody[] timeBodyArray = FindObjectsOfType(typeof(TimeBody)) as TimeBody[];
        foreach (TimeBody timeBody in timeBodyArray)
        {
            timeBody.Return();
        }
    }
}
