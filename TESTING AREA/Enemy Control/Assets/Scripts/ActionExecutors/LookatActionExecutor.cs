using UnityEngine;
using System.Collections;

public class LookatActionExecutor: BaseExecutor
{
    LookatAction lookatAction;
    Vector3 finalDirection;


    public override void SetAction(Action act)
    {
        base.SetAction(act);
        lookatAction = (LookatAction)act;

        string tId = lookatAction.targetId;

        if (tId != "player")
        {
            state.target = GameObject.Find(tId);
        }
        else if(!state.target.activeSelf)
        {
            state.target = enemyManager.SelectTarget();
        }

        finalDirection = state.target.transform.position - state.agent.transform.position;
        state.agent.Stop();
    }

    public override int Execute()
    {
        state.agent.transform.Rotate(new Vector3(0, 15, 0));

        if (Vector3.Angle(finalDirection, state.agent.transform.forward) < 25)
        {
            state.agent.transform.LookAt(state.target.transform.position);
            state.agent.Resume();
            return lookatAction.nextAction;
        }
        else
        {
            return Action.ACTION_NOT_FINISHED;
        }
    }
}
