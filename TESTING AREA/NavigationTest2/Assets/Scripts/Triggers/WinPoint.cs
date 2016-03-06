using UnityEngine;
using System.Collections;

public class WinPoint : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            mng.gameManager.PlayerWin();
        }
    }
}
