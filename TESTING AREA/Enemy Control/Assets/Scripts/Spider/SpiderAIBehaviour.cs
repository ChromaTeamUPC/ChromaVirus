using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderAIBehaviour : MonoBehaviour
{
    private AIBaseState currentState;

    private AIBaseState defaultState;
    private AIBaseState closeState;
    private AIBaseState attackChipState;

    //Test, remove
    private bool done = false;
    private float time;

	// Use this for initialization
	void Awake ()
    {
        defaultState = new SpiderAIDefaultState(this);
        closeState = new AIBaseState(this);
        attackChipState = new AIBaseState(this);
	}

    public void AIInit(List<Action> def, List<Action> close, List<Action> attack)
    {
        //Init states with lists
        defaultState.Init(def, defaultState);
        closeState.Init(close, defaultState);
        attackChipState.Init(attack, defaultState);

        ChangeState(defaultState);
    }	

	// Update is called once per frame
	void Update ()
    {
        AIBaseState newState = currentState.Update();

        if(newState != null)
        {
            ChangeState(newState);
        }

        //More conditions to change state
        if (time < 5)
            time += Time.deltaTime;
        else if (!done)
        {
            ChangeState(closeState);
            done = true;
        }
        
	}

    private void ChangeState(AIBaseState newState)
    {
        if (currentState != null) currentState.OnStateExit();
        currentState = newState;
        if (currentState != null) currentState.OnStateEnter();
    }
}
