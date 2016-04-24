using UnityEngine;
using System.Collections;

public class StaticVarsTest: MonoBehaviour{

    public static int foo = 0;

    void Awake()
    {
        Debug.Log("Awake");
    }

    void Start()
    {
        Debug.Log("Start");
    }
}
