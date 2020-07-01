using UnityEngine;

public class ActorFrame
{
    public Quaternion torsoRotation;
    public Quaternion headRotation;
    public Quaternion blockRotation;

    // INPUT

    public float mouseX;
    public float mouseY;
    public int moveSide;
    public int moveForward;

    public bool jump;
    public bool grab;
    public bool drop;
    public bool rotate;
    public bool shift;

    public ActorFrame(Quaternion _torsoRotation, Quaternion _headRotation, Quaternion _blockRotation, bool _jump, bool _grab, bool _drop, bool _rotate, bool _shift, float _mouseX, float _mouseY, int _moveSide, int _moveForward)
    {

        torsoRotation = _torsoRotation;
        headRotation = _headRotation;
        blockRotation = _blockRotation;
        
        jump = _jump;
        grab = _grab;
        drop = _drop;
        rotate = _rotate;
        shift = _shift;

        moveSide = _moveSide;
        moveForward = _moveForward;
        mouseX = _mouseX;
        mouseY = _mouseY;
    }
}