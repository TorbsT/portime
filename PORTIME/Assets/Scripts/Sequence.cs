using UnityEngine;

public class Sequence
{
    public int start;
    public int length;
    public int thisSequenceSkip;

    public Sequence(int _start, int _length, int _skip)
    {
        start = _start;
        length = _length;
        thisSequenceSkip = _skip;
    }
}
