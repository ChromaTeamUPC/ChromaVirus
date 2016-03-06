using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    //public GameObject shot;
    public float maxEnergy = 100;
    public float energyLostPerShot = 5;
    public float energyRecoveryPerSecond = 5;
    public float currentEnergy = 100;
    public float fireRate = 0.25f;
    public Transform shotSpawn; 
    public Material redMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;

    private ObjectPool shotObjectPool;
    private float nextFire;
    private ChromaColor currentColor;
    private Material currentMaterial;

    void Start()
    {
        currentEnergy = maxEnergy;
        shotObjectPool = mng.poolManager.shotPool;
        mng.eventManager.StartListening(EventManager.EventType.COLOR_CHANGED, ColorChanged);
        currentMaterial = redMaterial;
    }

    void ColorChanged(EventInfo eventInfo)
    {
        ColorEventInfo info = (ColorEventInfo)eventInfo;

        currentColor = info.newColor;

        switch(currentColor)
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


    // Update is called once per frame
    void Update () {
        if (Input.GetButton("P1_Fire") && Time.time > nextFire && currentEnergy >= energyLostPerShot)
        {
            nextFire = Time.time + fireRate;

            //Get a shot from pool
            GameObject shot = shotObjectPool.GetObject();
            

            if(shot != null)
            {
                shot.transform.position = shotSpawn.position;
                shot.transform.rotation = shotSpawn.rotation;
                shot.GetComponent<ShootMover>().color = currentColor;
                shot.GetComponent<Renderer>().material = currentMaterial;
                shot.SetActive(true);
            }

            currentEnergy -= energyLostPerShot;
        }

        if (currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime * energyRecoveryPerSecond;
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        }
    }
}
