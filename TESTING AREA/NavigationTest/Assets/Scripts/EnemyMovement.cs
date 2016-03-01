using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public Transform goal;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
	    agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
	
	// Update is called once per frame
	void Update () {
        agent.destination = goal.position;
    }
}
