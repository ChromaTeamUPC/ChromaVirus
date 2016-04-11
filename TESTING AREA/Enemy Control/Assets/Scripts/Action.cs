using UnityEngine;
using System.Collections;

public class Action
{
    public const int NEXT_ACTION = -1;
    public const int ACTION_NOT_FINISHED = -2;
    public const int LIST_FINISHED = -3;

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

    public enum Type
    {
        NONE,
        SELECT_TARGET,
        MOVE,
        ATTACK
    }

    public Type actionType;
    public int nextAction;

    public Action(int next)
    {
        actionType = Type.NONE;
        nextAction = next;
    }
}

public class SelectTargetAction: Action
{
    public string targetId; //TO REFACTOR: Not sure if needed

    public SelectTargetAction(string target, int next = Action.NEXT_ACTION): base(next)
    {
        actionType = Type.SELECT_TARGET;
        targetId = target;
    }
}

public class MoveAction: Action
{
    public string targetId;
    public float speed;
    public FocusType focusType;
    public OffsetType offsetType;
    public int angle;
    public float distance;
    
    public MoveAction(string target, float sp = 4f, int next = Action.NEXT_ACTION) : this(target, sp, FocusType.FIXED, OffsetType.POSITION_ZERO, 0, 0, next)
    {  
    }

    public MoveAction(string target, float sp, FocusType focusT, OffsetType offsetT, int ang = 0, float dist = 0f, int next = Action.NEXT_ACTION) : base(next)
    {
        actionType = Type.MOVE;
        targetId = target;
        speed = sp;
        focusType = focusT;
        offsetType = offsetT;
        angle = ang;
        distance = dist;
    }
}
