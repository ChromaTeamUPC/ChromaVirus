using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour
{
    public GameObject player1Zone;
    public Slider player1Health;
    public Slider player1Energy;
    public GameObject player1ExtraLife1;
    public GameObject player1ExtraLife2;

    public GameObject player2Zone;
    public Slider player2Health;
    public Slider player2Energy;
    public GameObject player2ExtraLife1;
    public GameObject player2ExtraLife2;

    private GameObject player1;
    private PlayerController player1Controller;
    private GameObject player2;
    private PlayerController player2Controller;

    private float referenceHealthFactor;
    private float referenceEnergyFactor;

    // Use this for initialization
    void Start ()
    {
        if(rsc.gameInfo.numberOfPlayers == 2)
        {
            player2Zone.SetActive(true);
        }
        else
        {
            player2Zone.SetActive(false);
        }

        player1 = rsc.gameInfo.player1;
        player1Controller = rsc.gameInfo.player1Controller;

        player2 = rsc.gameInfo.player2;
        player2Controller = rsc.gameInfo.player2Controller;

        referenceHealthFactor = player1Health.maxValue / player1Controller.maxHealth;
        referenceEnergyFactor = player1Energy.maxValue / player1Controller.maxEnergy;
    }

    void OnDestroy()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Player 1 update
        player1Health.value = player1Controller.Health * referenceHealthFactor;
        player1Energy.value = player1Controller.Energy * referenceEnergyFactor;

        if (player1Controller.Lives > 1)
            player1ExtraLife1.SetActive(true);
        else
            player1ExtraLife1.SetActive(false);

        if (player1Controller.Lives > 2)
            player1ExtraLife2.SetActive(true);
        else
            player1ExtraLife2.SetActive(false);


        //Player 2 update
        player2Health.value = player2Controller.Health * referenceHealthFactor;
        player2Energy.value = player2Controller.Energy * referenceEnergyFactor;

        if (player2Controller.Lives > 1)
            player2ExtraLife1.SetActive(true);
        else
            player2ExtraLife1.SetActive(false);

        if (player2Controller.Lives > 2)
            player2ExtraLife2.SetActive(true);
        else
            player2ExtraLife2.SetActive(false);
    }
}
