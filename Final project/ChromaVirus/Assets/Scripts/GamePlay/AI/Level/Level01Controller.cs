using UnityEngine;
using System.Collections;

public class Level01Controller : MonoBehaviour 
{
    public Transform player1StartPoint;
    public Transform player2StartPoint;

    public GameObject wallZone01;
    public GameObject wallZone02;
    public GameObject wallZone03;

    [SerializeField]
    private FadeSceneScript fadeScript;

	// Use this for initialization
	void Start () 
    {
        rsc.colorMng.Activate();

        if (rsc.gameInfo.player1Controller.Active)
        {
            rsc.gameInfo.player1.transform.position = player1StartPoint.position;
            rsc.gameInfo.player1.transform.SetParent(null);
            if (!rsc.gameInfo.player1.activeSelf)
                rsc.gameInfo.player1.SetActive(true);
        }
        if (rsc.gameInfo.numberOfPlayers == 2 && rsc.gameInfo.player2Controller.Active)
        {
            rsc.gameInfo.player2.transform.position = player2StartPoint.position;
            rsc.gameInfo.player2.transform.SetParent(null);
            if (!rsc.gameInfo.player2.activeSelf)
                rsc.gameInfo.player2.SetActive(true);
        }

        rsc.eventMng.StartListening(EventManager.EventType.ZONE_REACHED, ZoneReached);
        rsc.eventMng.StartListening(EventManager.EventType.ZONE_PLAN_FINISHED, ZonePlanFinished);

        fadeScript.StartFadingToClear();
	}

    void OnDestroy()
    {
        if (rsc.eventMng != null)
        {
            rsc.eventMng.StopListening(EventManager.EventType.ZONE_REACHED, ZoneReached);
            rsc.eventMng.StopListening(EventManager.EventType.ZONE_PLAN_FINISHED, ZonePlanFinished);
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    private void ZoneReached(EventInfo eventInfo)
    {
        ZoneReachedInfo info = (ZoneReachedInfo)eventInfo;
        rsc.enemyMng.StartPlan(info.zoneId);
    }

    private void ZonePlanFinished(EventInfo eventInfo)
    {
        ZonePlanEndedInfo info = (ZonePlanEndedInfo)eventInfo;
        switch (info.planId)
        {
            //open door 01
            case 101:
                wallZone01.SetActive(false);
                break;

            case 102:
                wallZone02.SetActive(false);
                wallZone03.SetActive(false);
                break;

            case 103:
                break;
        }
    }
}
