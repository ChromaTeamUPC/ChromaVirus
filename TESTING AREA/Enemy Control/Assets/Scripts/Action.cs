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

    public Action(Type n, Vector3 w, float s = 4.0f, int na = NEXT_ACTION)
    {
        name = n;
        waypoint = w;
        speed = s;
        nextAction = na;
    }

    public Type name;
    public Vector3 waypoint;
    public int nextAction;
    public float speed;
}
