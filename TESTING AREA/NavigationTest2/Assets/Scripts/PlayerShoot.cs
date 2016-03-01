using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;


    private float nextFire;
    private Material currentMaterial;

    void Start()
    {
        EventManager evMan = mng.eventManager;

        //evMan.StartListening(EventManager.EventType.COLOR_RED, ColorRed);
        //evMan.StartListening(EventManager.EventType.COLOR_GREEN, ColorGreen);
        //evMan.StartListening(EventManager.EventType.COLOR_BLUE, ColorBlue);
        //evMan.StartListening(EventManager.EventType.COLOR_YELLOW, ColorYellow);
        evMan.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
        currentMaterial = redMaterial;
    }

    void ColorChanged(EventInfo eventInfo)
    {
        ColorChangedEventInfo info = (ColorChangedEventInfo)eventInfo;

        switch(info.newColor)
        {
            case ChromaColor.RED:
                currentMaterial = redMaterial;
                break;
            case ChromaColor.GREEN:
                currentMaterial = greenMaterial;
                break;
            case ChromaColor.BLUE:
                currentMaterial = blueMaterial;
                break;
            case ChromaColor.YELLOW:
                currentMaterial = yellowMaterial;
                break;
        }
    }

    void ColorRed()
    {
        currentMaterial = redMaterial;
    }

    void ColorGreen()
    {
        currentMaterial = greenMaterial;
    }

    void ColorBlue()
    {
        currentMaterial = blueMaterial;
    }

    void ColorYellow()
    {
        currentMaterial = yellowMaterial;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject spawn = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
            spawn.GetComponent<Renderer>().material = currentMaterial;
        }
    }
}
