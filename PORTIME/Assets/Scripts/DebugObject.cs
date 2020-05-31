using UnityEngine;

public class DebugObject : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        Debug.Log("Pos:" + transform.position + " - V: " + rb.velocity + " - aV: " + rb.angularVelocity);
    }
}
