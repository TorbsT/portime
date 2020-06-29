using UnityEngine;

public class ActorFrame
{
    public Quaternion torsoRotation;
    public Quaternion headRotation;
    public Quaternion blockRotation;

    

    // INPUT

    public bool isJumping;
    public bool grab;
    public bool drop;
    public bool rotate;
    public bool shift;

    public ActorFrame(Quaternion _torsoRotation, Quaternion _headRotation, Quaternion _blockRotation, bool _isJumping, bool _grab, bool _drop, bool _rotate, bool _shift)
    {

        torsoRotation = _torsoRotation;
        headRotation = _headRotation;
        blockRotation = _blockRotation;
        
        isJumping = _isJumping;
        grab = _grab;
        drop = _drop;
        rotate = _rotate;
        shift = _shift;
    }
}