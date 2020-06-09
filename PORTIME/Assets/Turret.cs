using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform rotatorSocket;

    void Start()
    {
        //rotator = transform.Find("RotatorSocket");
    }

    void FixedUpdate()
    {
        rotatorSocket.Rotate(0, 0f, 1f, Space.Self);
    }
}
