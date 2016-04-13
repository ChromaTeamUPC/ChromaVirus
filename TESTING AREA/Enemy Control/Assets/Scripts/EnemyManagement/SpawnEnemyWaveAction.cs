using UnityEngine;
using System.Collections;

public class SpawnEnemyWaveAction : WaveAction
{
    public SpawnEnemyWaveAction(float del = 0f)
    {
        delay = del;
    }

    public override void Execute()
    {
        //Spawn enemy
    }

}
