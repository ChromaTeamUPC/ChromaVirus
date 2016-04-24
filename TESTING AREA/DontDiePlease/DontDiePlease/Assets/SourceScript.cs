using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SourceScript : MonoBehaviour {

    public MyCoolManager myCoolManager;
    public GameObject model;

	// Use this for initialization
	void Start ()
    {
        if (Storage.myCoolManager == null)
        {
            Storage.myCoolManager = myCoolManager;
            DontDestroyOnLoad(Storage.myCoolManager);
        }

        if (Storage.myCoolGameObject == null)
        {
            Storage.myCoolGameObject = Instantiate(model, Vector3.zero, Quaternion.identity) as GameObject;
            Storage.myCoolGameObject.name = "CoolCube";
            DontDestroyOnLoad(Storage.myCoolGameObject);
        }
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("level2");
        }
    }
	
}
