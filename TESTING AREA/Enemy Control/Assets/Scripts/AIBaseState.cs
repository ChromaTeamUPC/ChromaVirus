using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBaseState
{
    protected MonoBehaviour parent;
    public NavMeshAgent agent;

    protected List<Action> actionsList;
    protected int currentActionIndex;
    protected AIBaseState nextState;

    public GameObject target;

    protected BaseExecutor currentExecutor;
    protected SelectTargetExecutor selectExecutor;
    protected MoveActionExecutor moveExecutor;

    public AIBaseState(MonoBehaviour p)
    {
        parent = p;
        agent = parent.GetComponent<NavMeshAgent>();

        selectExecutor = new SelectTargetExecutor();
        selectExecutor.Init(this);
        moveExecutor = new MoveActionExecutor();
        moveExecutor.Init(this);

    }

    virtual public void Init(List<Action> actions, AIBaseState nextSt)
    {
        actionsList = actions;
        nextState = nextSt;
    }

    virtual public void OnStateEnter()
    {
        currentActionIndex = 0;
        UpdateExecutor();
    }
    virtual public void OnStateExit() { }

    virtual public AIBaseState Update()
    {
        int nextAction = currentExecutor.Execute();

        switch(nextAction)
        {
            case Action.LIST_FINISHED:
                return nextState;

            case Action.ACTION_NOT_FINISHED:
                //Do nothing here
                break;

            case Action.NEXT_ACTION:
                ++currentActionIndex;
                if (currentActionIndex == actionsList.Count)
                    currentActionIndex = 0;

                UpdateExecutor();
                break;

            default:
                currentActionIndex = nextAction;
                UpdateExecutor();
                break;
        }

        return null;
    }

    protected void UpdateExecutor()
    {
        Action action = actionsList[currentActionIndex];

        switch(action.actionType)
        {
            case Action.Type.SELECT_TARGET:
                currentExecutor = selectExecutor;          
                break;

            case Action.Type.MOVE:
                currentExecutor = moveExecutor;
                break;
        }

        currentExecutor.SetAction(action);
    }

}

