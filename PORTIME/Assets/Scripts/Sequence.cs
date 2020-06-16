using UnityEngine;

public class Sequence
{
    public int start;
    public int length;
    public int skip;

    public Sequence(int _start, int _length, int _skip)
    {
        start = _start;
        length = _length;
        skip = _skip;
    }
}
