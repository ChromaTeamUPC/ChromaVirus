using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    private List<Action> test1 = new List<Action>();

    void Awake()
    {
        test1.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp1"));
        test1.Add(new Action(Action.Type.MOVE, Action.OffsetType.AROUND, "Player", 90, 5.0f, Action.FocusType.CONTINUOUS));
        test1.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp3", 0, 0, Action.FocusType.FIXED, 4.0f, 0));
    }
}
