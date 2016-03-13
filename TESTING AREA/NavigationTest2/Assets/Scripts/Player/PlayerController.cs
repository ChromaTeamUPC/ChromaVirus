using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int maxLives = 3;
    public int maxHealth = 100;

    public float speed = 10;
    public float angularSpeed = 360;
    public float aimThreshold = 0.2f;

    public float maxEnergy = 100;
    public float energyLostPerShot = 5;
    public float energyRecoveryPerSecond = 5;
    public float fireRate = 0.25f;
    public Transform shotSpawn;

    //Private atributes
    private int playerId;
    private int currentLives;
    private int currentHealth;
    private float currentEnergy;
    private float nextFire;

    private string moveHorizontal;
    private string moveVertical;
    private string aimHorizontal;
    private string aimVertical;
    private string fire;
    private string dash;

    private Rigidbody rigidBody;
    private Transform cameraTransform;
    private ShotManager shotManager;

    //Properties
    public int Id { get { return playerId; } }
    public int Lives {  get { return currentLives; } }
    public int Health { get { return currentHealth; } }
    public int Energy { get { return (int)currentEnergy; } }

    //Unity methods
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        //Temporary call
        InitPlayer(1);
    }

    void Start ()
    {
        cameraTransform = mng.cameraManager.currentCamera.transform;
        shotManager = mng.shotManager;
        mng.eventManager.StartListening(EventManager.EventType.CAMERA_CHANGED, CameraChanged);
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        Shoot();
    }

    //Custom methods
    public void CameraChanged(EventInfo eventInfo)
    {
        CameraEventInfo info = (CameraEventInfo)eventInfo;
        cameraTransform = info.newCamera.transform;
    }

    public void InitPlayer(int playerNumber)
    {
        playerId = playerNumber;
        currentLives = maxLives;
        ResetPlayer();

        string player = "";
        switch(playerId)
        {
            case 1: player = "P1"; break;
            case 2: player = "P2"; break;
        }

        moveHorizontal = player + "_Horizontal";
        moveVertical = player + "_Vertical";
        aimHorizontal = player + "_AimHorizontal";
        aimVertical = player + "_AimVertical";
        fire = player + "_Fire";
        dash = player + "_Dash";

        mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_SPAWNED, new PlayerSpawnedEventInfo { player = this });
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    private void Move()
    {
        float h = Input.GetAxisRaw(moveHorizontal);
        float v = Input.GetAxisRaw(moveVertical);

        Vector3 displacement = new Vector3(h, 0f, v);

        //Get the Y rotation angle from the camera
        float camRotation = cameraTransform.rotation.eulerAngles.y;
        //Apply that rotation to the direction vector
        displacement = Quaternion.Euler(0, camRotation, 0) * displacement;

        //Add velocity and apply it
        displacement = displacement * speed * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + displacement);
    }

    private void Turn()
    {
        float h = Input.GetAxisRaw(aimHorizontal);
        float v = Input.GetAxisRaw(aimVertical);

        if (Mathf.Abs(v) >= aimThreshold || Mathf.Abs(h) >= aimThreshold)
        {
            Vector3 lookAt = new Vector3(h, 0, v);

            //Get the Y rotation angle from the camera
            float camRotation = cameraTransform.rotation.eulerAngles.y;
            //Apply that rotation to the direction vector
            lookAt = Quaternion.Euler(0, camRotation, 0) * lookAt;

            //Get the smoothed rotation quaternion
            Quaternion rotation = Quaternion.LookRotation(lookAt, Vector3.up);
            rotation = Quaternion.RotateTowards(rigidBody.rotation, rotation, angularSpeed * Time.deltaTime);
            //Apply it to rigidbody
            rigidBody.MoveRotation(rotation);
        }
    }

    private void Shoot()
    {
        if (Input.GetAxisRaw(fire) > 0.5f && Time.time > nextFire && currentEnergy >= energyLostPerShot)
        {
            nextFire = Time.time + fireRate;

            //Get a shot from pool
            GameObject shot = shotManager.GetShot();

            if (shot != null)
            {
                shot.transform.position = shotSpawn.position;
                shot.transform.rotation = shotSpawn.rotation;
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) currentHealth = 0;

        //Send event
        mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_DAMAGED, new PlayerDamagedEventInfo { damage = damage, currentHealth = currentHealth });

        if (currentHealth == 0)
        {
            mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_DIED, EventInfo.emptyInfo);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DeathZone")
        {
            TakeDamage(currentHealth);
        }
    }
}
