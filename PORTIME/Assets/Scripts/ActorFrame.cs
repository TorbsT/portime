using UnityEngine;

public class ActorFrame
{
    public Quaternion actorHead;

    // INPUT
    public float mouseX;
    public float mouseY;
    public float moveSide;
    public float moveForward;
    public bool jump;

    public ActorFrame(Quaternion _actorHead, float _mouseX, float _mouseY, float _moveSide, float _moveForward, bool _jump)
    {
        actorHead = _actorHead;

        mouseX = _mouseX;
        mouseY = _mouseY;
        moveSide = _moveSide;
        moveForward = _moveForward;
        jump = _jump;
    }
}
