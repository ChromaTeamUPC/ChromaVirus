using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptObjectPool<T> where T : MonoBehaviour
{
    //This defines which game object will be the parent of the pooled objects so they don't fill the hierarchy root
    public GameObject objectsParent;
    //This defines which kind of GameObject has the script attached to, so we know what to instantiate
    public GameObject objectWhereScriptIs; 
    
    public int poolSize = 100;

    //Use with caution
    public bool grow = false;

    private Queue<T> pool;

    public ScriptObjectPool()
    {
        Debug.Log("Script Object Pool created");
    }

    ~ScriptObjectPool()
    {
        Debug.Log("Script Object Pool destroyed");
    }

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
        {
            T pooledObject = pool.Dequeue();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
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
