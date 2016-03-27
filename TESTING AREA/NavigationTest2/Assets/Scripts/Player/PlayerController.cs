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
    private VoxelizationClient voxelization;

    private bool isFirstShot = true;

    const float maxSideOffset = 0.4f;
    const float minSideOffset = 0.2f;
    private float shotSideOffset = minSideOffset;
    private float sideOffsetVariation = -0.05f;

    public Light shotLight;


    //Properties
    public int Id { get { return playerId; } }
    public int Lives {  get { return currentLives; } }
    public int Health { get { return currentHealth; } }
    public int Energy { get { return (int)currentEnergy; } }

    //Unity methods
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        voxelization = GetComponent<VoxelizationClient>();

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

        gameObject.SetActive(true);
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
        shotLight.enabled = false;

        if (Input.GetAxisRaw(fire) > 0.1f && Time.time > nextFire && currentEnergy >= energyLostPerShot)
        {
            nextFire = Time.time + fireRate;
            shotLight.enabled = true;

            // check if it's first shot (single projectile)...
            if (isFirstShot)
            {
                //Get a shot from pool
                GameObject shot = shotManager.GetShot();
                shot.GetComponent<ShotMover>().damage *= 2;

                if (shot != null)
                {
                    shot.transform.position = shotSpawn.position;
                    shot.transform.rotation = shotSpawn.rotation;
                    shot.SetActive(true);
                }
                isFirstShot = false;
            }
            // ...or not (double projectile)
            else
            {
                //Get two shots from pool
                GameObject shot1 = shotManager.GetShot();
                GameObject shot2 = shotManager.GetShot();

                if (shot1 != null && shot2 != null)
                {
                    shot1.transform.rotation = shotSpawn.rotation;
                    shot1.transform.position = shotSpawn.position;
                    shot1.transform.Translate(new Vector3(shotSideOffset, 0, 0));
                    shot1.SetActive(true);

                    shot2.transform.rotation = shotSpawn.rotation;
                    shot2.transform.position = shotSpawn.position;
                    shot2.transform.Translate(new Vector3(-shotSideOffset, 0, 0));
                    shot2.SetActive(true);

                    if (shotSideOffset <= minSideOffset || shotSideOffset >= maxSideOffset)
                        sideOffsetVariation *= -1;

                    shotSideOffset += sideOffsetVariation;
                }
            }
            currentEnergy -= energyLostPerShot;
        }
        else if (Input.GetAxisRaw(fire) <= 0.1f)
        {
            isFirstShot = true;
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
            voxelization.CalculateVoxelsGrid();
            voxelization.SpawnVoxels();
            mng.eventManager.TriggerEvent(EventManager.EventType.PLAYER_DIED, new PlayerSpawnedEventInfo { player = this });
            gameObject.SetActive(false);
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
