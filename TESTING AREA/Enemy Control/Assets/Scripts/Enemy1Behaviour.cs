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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        actionList.Add(new Action(Action.Type.MOVE, wp1.position, 15.0f));
        actionList.Add(new Action(Action.Type.MOVE, wp2.position, 5.0f));
        actionList.Add(new Action(Action.Type.MOVE, wp3.position, 5.0f, 0));
    }
    
    void Update()
    {
        if (currentAction <= actionList.Count)
        {
            if (actionList[currentAction].name == Action.Type.MOVE)
            {
                if (!actionStarted)
                {
                    agent.destination = actionList[currentAction].waypoint;
                    agent.speed = actionList[currentAction].speed;
                    actionStarted = true;
                }
                else
                {
                    if (agent.remainingDistance <= 0.5)
                    {
                        if (actionList[currentAction].nextAction == Action.NEXT_ACTION)
                            currentAction++;
                        else
                            currentAction = actionList[currentAction].nextAction;

                        actionStarted = false;
                    }
                }
                

                //agent.destination = actionList[currentAction].waypoint;
            }
        }
    }
}
