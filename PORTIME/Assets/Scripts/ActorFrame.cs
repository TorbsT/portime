using UnityEngine;

public class ActorFrame
{
    public Quaternion torsoRotation;
    public Quaternion headRotation;

    // INPUT
    public float mouseX;
    public float mouseY;
    public float moveSide;
    public float moveForward;
    public bool jump;
    public bool grab;
    public bool drop;
    public bool run;

    public ActorFrame(Quaternion _torsoRotation, Quaternion _headRotation, float _mouseX, float _mouseY, float _moveSide, float _moveForward, bool _jump, bool _grab, bool _drop)
    {
        torsoRotation = _torsoRotation;
        headRotation = _headRotation;

        mouseX = _mouseX;
        mouseY = _mouseY;
        moveSide = _moveSide;
        moveForward = _moveForward;
        jump = _jump;
        grab = _grab;
        drop = _drop;
    }
}