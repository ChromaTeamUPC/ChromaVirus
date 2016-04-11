using UnityEngine;
using System.Collections;

public class WinPoint : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player1" || other.tag == "Player2")
        {
            mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_WIN, EventInfo.emptyInfo);
        }
    }
}
