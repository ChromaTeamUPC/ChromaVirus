using UnityEngine;
using System.Collections;

public class SelectTargetExecutor: BaseExecutor
{
    public override int Execute()
    {
        state.target = enemyManager.SelectTarget();
        return action.nextAction;
    }
}
