using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptObjectPool<T> where T : MonoBehaviour
{
    public GameObject objectsParent;
    public GameObject objectWhereScriptIs;
    
    public int poolSize = 100;

    //Use with caution
    public bool grow = false;

    private Queue<T> pool;

    // Use this for initialization
    public void Init()
    {
        Transform trans = objectsParent.transform;
        GameObject aux;

        pool = new Queue<T>(poolSize);
        for (int i = 0; i < poolSize; ++i)
        {
            aux = GameObject.Instantiate(objectWhereScriptIs);
            aux.SetActive(false);
            aux.transform.SetParent(trans);
            pool.Enqueue(aux.GetComponent<T>());
        }
    }

    public T GetObject()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        else if (grow)
        {
            //Grow will happen not now but when all the objects will be enqueued again and capacity will be full
            GameObject aux = GameObject.Instantiate(objectWhereScriptIs);
            aux.SetActive(false);
            aux.transform.SetParent(objectsParent.transform);
            return aux.GetComponent<T>();
        }
        else
            return null;
    }

    public void AddObject(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pool.Enqueue(pooledObject);
    }

    public int GetPoolCount()
    {
        return pool.Count;
    }
}
