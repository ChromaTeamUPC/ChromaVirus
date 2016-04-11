using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject player;
    private List<Action> defaultList = new List<Action>();
    private List<Action> closeList = new List<Action>();
    public SpiderAIBehaviour enemy;

    void Awake()
    {
        defaultList.Add(new MoveAction("wp1"));
        defaultList.Add(new SelectTargetAction("player"));
        defaultList.Add(new MoveAction("player", 4.0f, Action.FocusType.CONTINUOUS, Action.OffsetType.AROUND,  90, 5.0f));
        defaultList.Add(new MoveAction("wp3", 4.0f, Action.FocusType.FIXED, Action.OffsetType.POSITION_ZERO));

        closeList.Add(new MoveAction("wp2", 10f, Action.LIST_FINISHED));
    }

    void Start()
    {
        enemy.AIInit(defaultList, closeList, null);
    }

    public GameObject SelectTarget()
    {
        return player;
    }
}
