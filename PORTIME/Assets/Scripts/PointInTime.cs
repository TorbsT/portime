using UnityEngine;

public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public Quaternion actorAllRotation;

    public PointInTime(Vector3 _position, Quaternion _rotation, Vector3 _velocity, Quaternion _actorAllRotation)
    {
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
        actorAllRotation = _actorAllRotation;
    }
}
