using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int health = 30;
    public ChromaColor color = ChromaColor.RED;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        //mng.eventManager.TriggerEvent(EventManager.EventType.ENEMY_SPAWNED, new ColorEventInfo { newColor = color });
    }

    void OnDisable()
    {
        //mng.eventManager.TriggerEvent(EventManager.EventType.ENEMY_DIED, new ColorEventInfo { newColor = color });
    }

    public void ImpactedByShot(ChromaColor shotColor, int damage)
    {
        if (shotColor == color)
        {
            TakeDamage(damage);
        }
        //Else future behaviour like duplicate or increase health
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
