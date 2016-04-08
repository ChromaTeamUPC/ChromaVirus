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
    private int floorMask;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        floorMask = LayerMask.GetMask("Floor");

        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp1"));
        //actionList.Add(new Action(Action.Type.MOVE, Action.wpType.ABSOLUTE, "wp2"));
        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.AROUND, "Player", 90, 3.0f));
        actionList.Add(new Action(Action.Type.MOVE, Action.OffsetType.POSITION_ZERO, "wp3", 0));
    }
    
    void Update()
    {
        if (currentAction <= actionList.Count)
        {
            Action action = actionList[currentAction];  // for readibility

            if (action.actionType == Action.Type.MOVE)
            {
                if (!actionStarted) // this is less frequent, will change to "else" instead
                {
                    Vector3 tgtPosition = GameObject.Find(action.targetID).transform.position;

                    if (action.offsetType == Action.OffsetType.POSITION_ZERO)
                        agent.destination = tgtPosition;
                    else if (action.offsetType == Action.OffsetType.AROUND)
                    {
                        Vector3 offsetPosition = Camera.main.WorldToScreenPoint(tgtPosition);
                        offsetPosition.x += 10;

                        Ray camRay = Camera.main.ScreenPointToRay(offsetPosition);
                        RaycastHit raycastHit;

                        if (Physics.Raycast(camRay, out raycastHit, 100, floorMask))
                        {
                            Vector3 offsetVector = (offsetPosition - raycastHit.point);
                            offsetVector = offsetVector.normalized;
                            offsetVector *= action.distance;
                            
                            offsetPosition += offsetVector;
                            agent.destination = offsetPosition;
                        }

                    }

                    agent.speed = action.speed;
                    actionStarted = true;
                }
                else
                {
                    if (agent.remainingDistance <= 0.5)
                    {
                        if (action.nextAction == Action.NEXT_ACTION)
                            currentAction++;
                        else
                            currentAction = action.nextAction;

                        actionStarted = false;
                    }
                }
            }
        }
    }
}
