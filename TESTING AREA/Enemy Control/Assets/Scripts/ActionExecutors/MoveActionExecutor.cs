using UnityEngine;
using System.Collections;

public class MoveActionExecutor: BaseExecutor
{
    MoveAction moveAction;
    private Vector3 direction;
    
    public override void SetAction(Action act)
    {
        base.SetAction(act);
        moveAction = (MoveAction)act;

        string tId = moveAction.targetId;

        if (tId != "player")
        {
            state.target = GameObject.Find(tId);
        }
        else if(!state.target.activeSelf)
        {
            state.target = enemyManager.SelectTarget();
        }

        switch(moveAction.offsetType)
        {
            case Action.OffsetType.POSITION_ZERO:
                direction = new Vector3(0, 0, 0);
                break;
            case Action.OffsetType.AROUND1:
                direction = new Vector3(0, 0, 1);
                direction = Quaternion.Euler(0, moveAction.angle, 0) * direction;
                direction *= moveAction.distance;
                break;
            case Action.OffsetType.AROUND2:
                direction = (state.agent.transform.position - state.target.transform.position).normalized;
                direction = Quaternion.Euler(0, moveAction.angle, 0) * direction;
                direction *= moveAction.distance;
                break;
        }

        if (moveAction.inertia == false)
            state.agent.acceleration = 1000;
        else
            state.agent.acceleration = 50;

        state.agent.speed = moveAction.speed;
        state.agent.destination = state.target.transform.position + direction;
    }

    public override int Execute()
    {
        if (moveAction.focusType == Action.FocusType.CONTINUOUS)
        {
            if(moveAction.targetId == "player" && !state.target.activeSelf)
            {
                state.target = enemyManager.SelectTarget();
            }

            if (moveAction.offsetType == Action.OffsetType.AROUND2)
            {
                direction = (state.agent.transform.position - state.target.transform.position).normalized;
                direction = Quaternion.Euler(0, moveAction.angle, 0) * direction;
                direction *= moveAction.distance;
            }

            state.agent.destination = state.target.transform.position + direction;
        }

        if (state.agent.hasPath && state.agent.remainingDistance <= 1f)
        {
            if (moveAction.inertia == false)
                state.agent.velocity = Vector3.zero;

            return moveAction.nextAction;
        }
        else
        {
            return Action.ACTION_NOT_FINISHED;
        }
    }
}
