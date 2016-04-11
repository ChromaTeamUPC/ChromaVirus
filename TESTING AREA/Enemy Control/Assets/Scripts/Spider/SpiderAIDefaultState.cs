using UnityEngine;
using System.Collections;

public class SpiderAIDefaultState : AIBaseState {

    public SpiderAIDefaultState(MonoBehaviour p): base (p)
    {
        currentActionIndex = 0;
    }

    public override void OnStateEnter()
    {
        UpdateExecutor();
    }
}
