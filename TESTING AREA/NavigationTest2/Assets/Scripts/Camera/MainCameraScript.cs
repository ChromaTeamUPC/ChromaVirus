using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
    public Transform target1;
    public Transform target2;

    public float smoothing = 5f;

    private Vector3 offset;

    void Start()
    {
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_SPAWNED, PlayerSpawned);
    }

    public void PlayerSpawned(EventInfo eventInfo)
    {
        PlayerController playerController = ((PlayerSpawnedEventInfo)eventInfo).player;

        switch(playerController.Id)
        {
            case 1: target1 = playerController.transform; break;
            case 2: target2 = playerController.transform; break;
        }
        offset = transform.position - target1.position;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (target1 != null)
        {
            Vector3 targetCamPos = target1.position + offset;
            //targetCamPos.y = transform.position.y; //Y Locked
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}
