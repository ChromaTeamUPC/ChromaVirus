using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int maxHealthth = 30;
    public ChromaColor color = ChromaColor.RED;

    private int currentHealth;
    private VoxelizationClient voxelization;
    private ObjectPool enemyPool;

	// Use this for initialization
	void Awake ()
    {     
        voxelization = GetComponent<VoxelizationClient>();      
	}

    void Start()
    {
        enemyPool = mng.poolManager.aracnoBotPool;
    }

    void OnEnable()
    {
        currentHealth = maxHealthth;
        //voxelization.SetMaterial(color);
        mng.eventManager.TriggerEvent(EventManager.EventType.ENEMY_SPAWNED, new ColorEventInfo { newColor = color });
    }

    void OnDisable()
    {
        mng.eventManager.TriggerEvent(EventManager.EventType.ENEMY_DIED, new ColorEventInfo { newColor = color });
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
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            voxelization.SetMaterial(color);
            voxelization.CalculateVoxelsGrid();
            voxelization.SpawnVoxels();
            enemyPool.AddObject(gameObject);
        }
    }
}
