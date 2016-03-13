﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{ 
    public Text player1HealthTxt;
    public Text player1EnergyTxt;

    public Text player2HealthTxt;
    public Text player2EnergyTxt; 

    public Text win;
    public Text gameOver;

    private PlayerController player1;
    private PlayerController player2;

    // Use this for initialization
    public void Init ()
    {
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_SPAWNED, PlayerSpawned);
    }

    void PlayerSpawned(EventInfo eventInfo)
    {
        PlayerController player = ((PlayerSpawnedEventInfo)eventInfo).player;
        switch(player.Id)
        {
            case 1:
                player1 = player;
                player1HealthTxt.enabled = true;
                player1EnergyTxt.enabled = true;
                break;

            case 2:
                player2 = player;
                player2HealthTxt.enabled = true;
                player2EnergyTxt.enabled = true;
                break;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(player1 != null)
        {
            player1HealthTxt.text = "Life: " + player1.Health;
            player1EnergyTxt.text = "Energy: " + player1.Energy;
        }


        if (player2 != null)
        {
            player2HealthTxt.text = "Life: " + player2.Health;
            player2EnergyTxt.text = "Energy: " + player2.Energy;
        }
    }
}
