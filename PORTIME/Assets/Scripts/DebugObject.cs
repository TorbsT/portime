using System.Collections.Generic;
using UnityEngine;

public class DebugObject : MonoBehaviour
{
    Rigidbody rb;

    //List<BasicFrame> basicHistory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //basicHistory = new List<BasicFrame>();
        Debug.Log("Initial: " + transform.position + " " + rb.velocity + " " + rb.angularVelocity);
    }


    void FixedUpdate()
    {
        Debug.Log("Pos:" + transform.position + " - V: " + rb.velocity + " - aV: " + rb.angularVelocity);
    }
}
