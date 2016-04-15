using UnityEngine;
using System.Collections;

public class Action
{
    public const int NEXT_ACTION = -1;
    public const int ACTION_NOT_FINISHED = -2;
    public const int LIST_FINISHED = -3;
    public const bool INERTIA_NO = false;
    public const bool INERTIA_YES = true;
    public const bool WARNING_NO = false;
    public const bool WARNING_YES = true;


    public enum OffsetType
    {
        POSITION_ZERO,      // same coordinates as the waypoint
        AROUND_1,            // around the target, at some degrees (relative to scenario) and distance 
        AROUND_2             // around the target, at some degrees (relative to enemy) and distance 
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
        SPIDER_ATTACK_1,
        LOOKAT
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
    public bool inertia;           // if set to false the enemy stops when arriving to the waypoint, else, the enemy may continue moving for a while because the inertia
    
    public MoveAction(string target, float sp = 4f, bool ine = INERTIA_YES, int next = Action.NEXT_ACTION) : this(target, sp, FocusType.FIXED, OffsetType.POSITION_ZERO, 0, 0, ine, next)
    {  
    }

    public MoveAction(string target, float sp, FocusType focusT, OffsetType offsetT, int ang = 0, float dist = 0f, bool ine = INERTIA_YES, int next = Action.NEXT_ACTION) : base(next)
    {
        actionType = Type.MOVE;
        targetId = target;
        speed = sp;
        focusType = focusT;
        offsetType = offsetT;
        angle = ang;
        distance = dist;
        inertia = ine;
    }
}

public class LookatAction : Action
{
    public string targetId;

    public LookatAction(string target, int next = Action.NEXT_ACTION) : base(next)
    {
        actionType = Type.LOOKAT;
        targetId = target;
    }
}

public class SpiderAttack1Action : Action
{
    public float delay;     // delay between attacks
    public bool warning;    // if true, the spider flashes an instant before attack
    public SpiderAttack1Action(bool warn = WARNING_YES, float dly = 1.0f, int next = Action.NEXT_ACTION) : base(next)
    {
        actionType = Type.SPIDER_ATTACK_1;
        warning = warn;
        delay = dly;
    }
}