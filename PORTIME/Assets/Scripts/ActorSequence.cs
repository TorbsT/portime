using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSequence
{
    public int start;

    public List<BasicFrame> basicHistory;
    public List<ActorFrame> actorHistory;
    public ActorSequence(int _start, List<BasicFrame> _basicHistory, List<ActorFrame> _actorHistory)
    {
        start = _start;
        basicHistory = _basicHistory;
        actorHistory = _actorHistory;
    }

}
