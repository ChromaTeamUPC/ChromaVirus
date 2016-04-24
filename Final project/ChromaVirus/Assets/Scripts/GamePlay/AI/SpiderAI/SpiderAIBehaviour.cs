using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderAIBehaviour : MonoBehaviour
{
    public int maxHealth = 30;
    public int biteDamage = 10;

    [HideInInspector]
    public ChromaColor color;

    private int currentHealth;
    private VoxelizationClient voxelization;
    [HideInInspector]
    public Renderer spiderRenderer;

    private AIBaseState currentState;

    private AIBaseState defaultState;
    private AIBaseState closeState;
    private AIBaseState attackChipState;

    [HideInInspector]
    public float timeSinceLastAttack;
    [HideInInspector]
    public bool biting;

    //Test, remove
    private bool done = true;
    private float time;

	// Use this for initialization
	void Awake ()
    {
        defaultState = new SpiderAIDefaultState(this);
        closeState = new AIBaseState(this);
        attackChipState = new AIBaseState(this);
        voxelization = GetComponent<VoxelizationClient>();
        spiderRenderer = GetComponentInChildren<Renderer>();
    }

    public void AIInit(List<AIAction> def, List<AIAction> close, List<AIAction> attack)
    {
        currentHealth = maxHealth;

        //Init states with lists
        defaultState.Init(def, defaultState);
        closeState.Init(close, defaultState);
        attackChipState.Init(attack, defaultState);    
    }

    public void Spawn(Transform spawnPoint)
    {
        timeSinceLastAttack = 100f;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        ChangeState(defaultState);

        ColorEventInfo.eventInfo.newColor = color;
        rsc.eventMng.TriggerEvent(EventManager.EventType.ENEMY_SPAWNED, ColorEventInfo.eventInfo);
    }

    public void BiteDone()
    {
        biting = false;
    }

    private void Die()
    {
        ColorEventInfo.eventInfo.newColor = color;
        rsc.eventMng.TriggerEvent(EventManager.EventType.ENEMY_DIED, ColorEventInfo.eventInfo);
        rsc.poolMng.spiderPool.AddObject(gameObject);
    }

    public void ImpactedByShot(ChromaColor shotColor, int damage)
    {
        if (shotColor == color)
        {
            TakeDamage(damage);
        }
        //Else future behaviour like duplicate or increase health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            voxelization.SetMaterial(color);
            voxelization.CalculateVoxelsGrid();
            voxelization.SpawnVoxels();
            Die();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        timeSinceLastAttack += Time.deltaTime;

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
