using UnityEngine;

public class ActorFrame
{
    public Quaternion actorAllRotation;

    // INPUT
    public float mouseX;
    public float mouseY;
    public float moveSide;
    public float moveForward;
    public bool jump;

    public ActorFrame(Quaternion _actorAllRotation, float _mouseX, float _mouseY, float _moveSide, float _moveForward, bool _jump)
    {
        actorAllRotation = _actorAllRotation;

        mouseX = _mouseX;
        mouseY = _mouseY;
        moveSide = _moveSide;
        moveForward = _moveForward;
        jump = _jump;
    }
}
