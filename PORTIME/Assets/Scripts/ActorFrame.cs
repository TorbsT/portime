using UnityEngine;

public class ActorFrame
{
    public Quaternion torsoRotation;

    // INPUT
    public float mouseX;
    public float mouseY;
    public float moveSide;
    public float moveForward;
    public bool jump;

    public ActorFrame(Quaternion _torsoRotation, float _mouseX, float _mouseY, float _moveSide, float _moveForward, bool _jump)
    {
        torsoRotation = _torsoRotation;

        mouseX = _mouseX;
        mouseY = _mouseY;
        moveSide = _moveSide;
        moveForward = _moveForward;
        jump = _jump;
    }
}
