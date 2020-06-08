using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float timeLeft;

    readonly float totalTime = 120f;

    private void Awake()
    {
        timeLeft = totalTime;
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
    }
}
