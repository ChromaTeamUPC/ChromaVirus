using UnityEngine;
using System.Collections;

public class MyCoolManager : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        Debug.Log("MyCoolManager created!");
	}

    public void DoSomeCoolStuff()
    {
        Storage.myCoolGameObject.transform.Translate(new Vector3(Random.Range(-1f, 1f), 0f, 0f));   
    }
	
	
}
