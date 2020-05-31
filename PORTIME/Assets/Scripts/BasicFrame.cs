using UnityEngine;

public class BasicFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public BasicFrame(Vector3 _position, Quaternion _rotation, Vector3 _velocity, Vector3 _angularVelocity)
    {
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
        angularVelocity = _angularVelocity;
    }
}
