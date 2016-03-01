using UnityEngine;
using System.Collections;

public class ShootMover : MonoBehaviour {

    public int speed;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
