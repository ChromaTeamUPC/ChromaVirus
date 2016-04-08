using UnityEngine;
using System.Collections;

public class Action
{
    public const int NEXT_ACTION = -1;
    public const int NOT_FINISHED = -2;

    public enum Type
    {
        MOVE,
        ATTACK
    }

    public enum OffsetType
    {
        POSITION_ZERO,      // same coordinates as the waypoint
        AROUND              // around the target, at some degrees and distance
    }

    public enum FocusType
    {
        FIXED,              // the waypoint is calculated only the first time
        CONTINUOUS          // the waypoint calculation is refreshed every frame
    }

    public Action(Type at, OffsetType off, string id, int an = 0, float dst = 0, FocusType ft = FocusType.FIXED, float s = 4.0f, int na = NEXT_ACTION)
    {
        actionType = at;
        offsetType = off;
        targetID = id;
        angle = an;
        distance = dst;
        speed = s;
        nextAction = na;
        focusType = ft;
    }

    public Type actionType;
    public OffsetType offsetType;
    public string targetID;         // an ID for an absolute waypoint
    public int nextAction;   
    public int angle;
    public float distance;
    public FocusType focusType;
    public float speed;
}
