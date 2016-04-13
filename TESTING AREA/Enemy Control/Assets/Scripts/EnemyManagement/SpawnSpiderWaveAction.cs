using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnSpiderWaveAction : WaveAction
{
    //ChromaColor color;
    Transform spawnPoint;
    List<Action> defaultActions;
    List<Action> closeActions;
    List<Action> attackChipActions;

    public SpawnSpiderWaveAction(/*ChromaColor c, */ Transform spPoint, List<Action> def, List<Action> close, List<Action> att, float del = 0f)
    {
        //color = c;
        spawnPoint = spPoint;
        defaultActions = def;
        closeActions = close;
        attackChipActions = att;
        delay = del;
    }

    public override void Execute()
    {
        //Spawn enemy
        GameObject spider;
        //spider = mng.PoolManager.SpiderPool.get...
        spider = GameObject.Instantiate(GameObject.Find("Enemy"));
        spider.transform.position = spawnPoint.position;
        spider.transform.rotation = spawnPoint.rotation;

        SpiderAIBehaviour behaviour = spider.GetComponent<SpiderAIBehaviour>();
        behaviour.AIInit(defaultActions, closeActions, attackChipActions);
        spider.SetActive(true);
    }

}