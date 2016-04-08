using UnityEngine;
using System.Collections;

public class Action
{
    public const int NEXT_ACTION = -1;

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

    public Action(Type at, OffsetType off, string id, int an = 0, float dst = 0, float s = 4.0f, int na = NEXT_ACTION)
    {
        actionType = at;
        offsetType = off;
        targetID = id;
        angle = an;
        distance = dst;
        speed = s;
        nextAction = na;
    }

    public Type actionType;
    public OffsetType offsetType;
    public Vector3 waypoint;        // provisional
    public string targetID;         // an ID for an absolute waypoint
    public int nextAction;
    public float speed;
    public int angle;
    public float distance;
}
