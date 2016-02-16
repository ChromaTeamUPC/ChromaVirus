using UnityEngine;
using System.Collections;

public class SpawnSphere : MonoBehaviour {
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
    public int zMin;
    public int zMax;

    public GameObject sphere;
    public KeyCode spawnKey;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(spawnKey))
        {
            float x = Random.Range(xMin, xMax);
            float y = Random.Range(yMin, yMax);
            float z = Random.Range(zMin, zMax);
            Vector3 pos = new Vector3(x, y, z);
            Instantiate(sphere, pos, Random.rotation);
        }
    }
}
