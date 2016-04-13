using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZonePlan {

    public int enemiesThreshold;
    public List<WaveAction> initialActions;
    public List<List<WaveAction>> triggerWaves;
    public Queue<List<WaveAction>> sequentialWaves;

    public ZonePlan()
    {
        enemiesThreshold = 3;
        initialActions = new List<WaveAction>();
        triggerWaves = new List<List<WaveAction>>();
        sequentialWaves = new Queue<List<WaveAction>>();
    }
}
