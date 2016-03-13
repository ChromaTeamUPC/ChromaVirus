using UnityEngine;
using System.Collections;

public class ShootMover : MonoBehaviour {

    public int speed;
    public int damage;
    public ChromaColor color;

	// Use this for initialization
	void OnEnable () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.ImpactedByShot(color, damage);
        }

        if (other.tag != "Player" && other.tag != "DestroyerBoundary" && other.tag != "Elevator")
            ReturnToPool();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "DestroyerBoundary")
            ReturnToPool();
    }

    void ReturnToPool()
    {
        mng.poolManager.shotPool.AddObject(gameObject);
    }
}
