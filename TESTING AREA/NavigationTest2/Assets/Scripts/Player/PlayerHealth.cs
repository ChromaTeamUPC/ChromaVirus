using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public int health = 100;

    public void Start()
    {
        mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_SPAWNED, new PlayerDamagedEventInfo { damage = 0, currentHealth = health });
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) health = 0;

        //Send event
        mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_DAMAGED, new PlayerDamagedEventInfo { damage = damage, currentHealth = health });

        if (health == 0)
        {
            mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_DIED, EventInfo.emptyInfo);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            TakeDamage(health);
        }     
    }
}
