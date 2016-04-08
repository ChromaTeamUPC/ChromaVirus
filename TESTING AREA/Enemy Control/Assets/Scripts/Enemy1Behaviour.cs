using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy1Behaviour : MonoBehaviour {

    public Transform wp1;
    public Transform wp2;
    public Transform wp3;

    private List<Action> actionList = new List<Action>();
    private int currentAction = 0;
    private NavMeshAgent agent;
    private bool actionStarted = false;

    private Transform target;
    private Vector3 direction;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp1"));
        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.AROUND, "Player", 90, 5.0f, Action.FocusType.CONTINUOUS));
        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp3", 0, 0, Action.FocusType.FIXED, 4.0f, 0));
    }
    
    void Update()
    {
        if (currentAction < actionList.Count)
        {
            Action action = actionList[currentAction];  // for readibility

            if (action.actionType == Action.Type.MOVE)
            {
                if (!actionStarted) // this is less frequent, will change to "else" instead
                {
                    target = GameObject.Find(action.targetID).transform;

                    if (action.offsetType == Action.OffsetType.POSITION_ZERO)
                        direction = new Vector3(0, 0, 0);
                    else if (action.offsetType == Action.OffsetType.AROUND)
                    {
                        direction = new Vector3(0, 0, 1);
                        direction = Quaternion.Euler(0, action.angle, 0) * direction;
                        direction *= action.distance;
                    }

                    agent.destination = target.position + direction;                  
                    agent.speed = action.speed;
                    actionStarted = true;
                }
                else
                {
                    if (action.focusType == Action.FocusType.CONTINUOUS)
                    {
                        agent.destination = target.position + direction;
                    }

                    if (agent.remainingDistance <= 0.5)
                    {
                        if (action.nextAction == Action.NEXT_ACTION)
                        {
                            currentAction++;
                            if (currentAction == actionList.Count)
                                currentAction = 0;
                        }
                        else
                            currentAction = action.nextAction;

                        actionStarted = false;
                    }                   
                }
            }
        }
    }
}
