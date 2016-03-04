using UnityEngine;
using System.Collections;

public class GodCameraScript : MonoBehaviour {

    public int speed = 10;
    public DebugKeys keys;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(keys.godCameraRight))
        {
            Vector3 displacement = transform.right * Time.deltaTime * speed;
            transform.position = transform.position + displacement;
        }
        else if (Input.GetKey(keys.godCameraLeft))
        {
            Vector3 displacement = transform.right * Time.deltaTime * speed * -1;
            transform.position = transform.position + displacement;
        }
        if (Input.GetKey(keys.godCameraForward))
        {
            Vector3 displacement = transform.forward * Time.deltaTime * speed;
            transform.position = transform.position + displacement;
        }
        else if (Input.GetKey(keys.godCameraBackward))
        {
            Vector3 displacement = transform.forward * Time.deltaTime * speed * -1;
            transform.position = transform.position + displacement;
        }
    }
}
