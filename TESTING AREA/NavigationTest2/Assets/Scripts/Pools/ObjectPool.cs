using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {

    public GameObject pooledObject;
    public int poolSize = 100;

    private Queue<GameObject> pool;

	// Use this for initialization
	void Awake () {
        Transform trans = transform;
        GameObject aux;
        pool = new Queue<GameObject>(poolSize);
        for (int i = 0; i < poolSize; ++i)
        {
            aux = Instantiate(pooledObject);
            aux.SetActive(false);
            aux.transform.SetParent(trans);
            pool.Enqueue(aux);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
            return pool.Dequeue();
        else
            return null;
    }

    public void AddObject(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
        pool.Enqueue(pooledObject);
    }

    public int GetPoolCount()
    {
        return pool.Count;
    }
}
