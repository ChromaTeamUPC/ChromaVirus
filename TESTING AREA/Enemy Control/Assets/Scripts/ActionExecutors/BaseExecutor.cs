using UnityEngine;
using System.Collections;

public class BaseExecutor
{
    protected AIBaseState state;
    protected Action action;
    protected EnemyManager enemyManager;

    public BaseExecutor()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    public void Init(AIBaseState parent)
    {
        state = parent;
    }

    public virtual void SetAction(Action act)
    {
        action = act;
    }

    public virtual int Execute() { return Action.ACTION_NOT_FINISHED; }
}
