using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    public GameObject player;
    private static List<Action> level01spiderDefault01 = new List<Action>();
    private static List<Action> closeList = new List<Action>();
    private static bool listsInit = false;
    public SpiderAIBehaviour enemy;

    //Plan zone
    private static ZonePlan plan0101;
    //...

    private ZonePlan currentPlan;
    public int enemies;
    private int spawners;
    private int turrets;
    private int executingWaves;

    void Awake()
    {
        if (!listsInit)
        {
            //Init behaviour actions lists
            level01spiderDefault01.Add(new MoveAction("wp2", 5.0f));
            level01spiderDefault01.Add(new SelectTargetAction("player"));
            level01spiderDefault01.Add(new MoveAction("player", 5.0f, Action.FocusType.CONTINUOUS, Action.OffsetType.AROUND2, 15, 5, Action.INERTIA_NO));
            level01spiderDefault01.Add(new MoveAction("wp3", 5.0f, Action.FocusType.FIXED, Action.OffsetType.POSITION_ZERO, 0, 0, Action.INERTIA_NO));

            closeList.Add(new MoveAction("wp2", 10f, true, Action.LIST_FINISHED));
            
            //Init zone plans
            //plan0101
            plan0101 = new ZonePlan();
            List<WaveAction> wave01 = new List<WaveAction>();
            wave01.Add(new SpawnSpiderWaveAction(GameObject.Find("wp1").transform, level01spiderDefault01, closeList, null));
            wave01.Add(new SpawnSpiderWaveAction(GameObject.Find("wp1").transform, level01spiderDefault01, closeList, null, 0f));
            wave01.Add(new SpawnSpiderWaveAction(GameObject.Find("wp1").transform, level01spiderDefault01, closeList, null, 0f));
            plan0101.sequentialWaves.Enqueue(wave01);

            //List<WaveAction> wave02 = new List<WaveAction>();
            //wave02.Add(new SpawnSpiderWaveAction(GameObject.Find("wp1").transform, level01spiderDefault01, closeList, null, 2f));
            //wave02.Add(new SpawnSpiderWaveAction(GameObject.Find("wp2").transform, level01spiderDefault01, closeList, null, 1f));
            //wave02.Add(new SpawnSpiderWaveAction(GameObject.Find("wp3").transform, level01spiderDefault01, closeList, null, 1f));
            //plan0101.sequentialWaves.Enqueue(wave02);

            listsInit = true;
        }


    }

    void Start()
    {
        enemy.AIInit(level01spiderDefault01, closeList, null);
        //StartPlan(0101);
    }

    public void StartPlan(int zoneId)
    {
        if (currentPlan != null)
        {
            Debug.Log("There is a current plan active");
            return;
        }

        switch(zoneId)
        {
            case 0101:
                currentPlan = plan0101;
                break;
        }

        enemies = 0;
        spawners = 0;
        turrets = 0;
        executingWaves = 0;

        foreach (WaveAction action in currentPlan.initialActions)
        {
            action.Execute();
        }

        if (currentPlan.sequentialWaves.Count > 0)
        {
            StartCoroutine(CastWave(currentPlan.sequentialWaves.Dequeue()));
        }
    }

    private IEnumerator CastWave(List<WaveAction> actions)
    {
        ++executingWaves;

        foreach(WaveAction action in actions)
        {
            if (action.Delay() > 0)
            {
                yield return new WaitForSeconds(action.Delay());
            }
            action.Execute();
        }

        --executingWaves;
    }

    private bool PlanEnded()
    {
        if (currentPlan == null)
            return true;

        return enemies + spawners + turrets + executingWaves + currentPlan.sequentialWaves.Count == 0;
    }

    void Update()
    {
        
        if(currentPlan != null)
        {
            if(PlanEnded())
            {
                //Send event plan finished
                currentPlan = null;
            }
            else
            {
                if(enemies < currentPlan.enemiesThreshold && executingWaves == 0 && currentPlan.sequentialWaves.Count > 0)
                {
                    StartCoroutine(CastWave(currentPlan.sequentialWaves.Dequeue()));
                }
            }
        }
    }

    public GameObject SelectTarget()
    {
        return player;
    }


}
