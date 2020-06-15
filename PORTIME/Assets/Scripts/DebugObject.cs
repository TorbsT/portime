using System.Collections.Generic;
using UnityEngine;

public class DebugObject : MonoBehaviour
{
    Rigidbody rb;
    GameMaster gameMaster;
    //List<BasicFrame> basicHistory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }


    void FixedUpdate()
    {
        if (gameMaster.framesPlayed >= 1 && gameMaster.framesPlayed <= 3)
            Debug.Log("Obj: " + name + ", frames played: " + gameMaster.framesPlayed + ", Pos:" + transform.position);
    }
}
