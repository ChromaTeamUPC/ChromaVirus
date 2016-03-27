using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {

    public int speed;
    public int damage;
    private int defaultDamage;
    public ChromaColor color;

	// Use this for initialization
	void OnEnable () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        defaultDamage = damage;
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
        damage = defaultDamage;
        mng.poolManager.shotPool.AddObject(gameObject);
    }
}
