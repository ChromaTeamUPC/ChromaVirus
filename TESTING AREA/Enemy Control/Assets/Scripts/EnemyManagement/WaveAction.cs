using UnityEngine;
using System.Collections;

public class WaveAction
{
    protected float delay = 0f;

    public float Delay()
    {
        return delay;
    }

    public virtual void Execute()
    {
    }
}
