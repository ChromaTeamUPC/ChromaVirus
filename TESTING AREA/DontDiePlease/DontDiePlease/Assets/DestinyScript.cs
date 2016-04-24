using UnityEngine;
using System.Collections;

public class DestinyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Storage.myCoolGameObject.transform.position = new Vector3(1, -3, 0);

        //Alternative if we don't use some kind of "storage class"
        //GameObject.Find("CoolCube").transform.position = new Vector3(1, -5, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Storage.myCoolManager.DoSomeCoolStuff();

            //Alternative if we don't use some kind of "storage class"
            //GameObject.Find("MyCoolManager").GetComponent<MyCoolManager>().DoSomeCoolStuff();
        }
    }
	
}
