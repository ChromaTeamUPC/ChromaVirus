using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public int maxLives = 3;
    public int maxHealth = 100;

    public float speed = 10;
    public float moveThreshold = 0.2f;
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
    private Camera currentCamera;
    private MainCameraScript mainCameraScript;
    private ShotManager shotManager;
    private VoxelizationClient voxelization;

    private bool isFirstShot = true;

    private const float maxSideOffset = 0.4f;
    private const float minSideOffset = 0.2f;
    private float shotSideOffset = minSideOffset;
    private float sideOffsetVariation = -0.05f;

    public Light shotLight;

    private int playerRayCastMask;
    private float camRayLength = 100f;

    //Properties
    public int Id { get { return playerId; } }
    public int Lives {  get { return currentLives; } }
    public int Health { get { return currentHealth; } }
    public int Energy { get { return (int)currentEnergy; } }

    //Unity methods
    void Awake()
    {
        playerRayCastMask = LayerMask.GetMask("PlayerRayCast");
        rigidBody = GetComponent<Rigidbody>();
        voxelization = GetComponent<VoxelizationClient>();
    }

    void Start ()
    {
        currentCamera = mng.cameraManager.currentCamera;
        mainCameraScript = currentCamera.GetComponent<MainCameraScript>();
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
        currentCamera = ((CameraEventInfo)eventInfo).newCamera;
        mainCameraScript = currentCamera.GetComponent<MainCameraScript>();
    }

    public void InitPlayer(int playerNumber)
    {
        playerId = playerNumber;
        currentLives = maxLives;


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

        ResetPlayer();
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

        if (Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0)
        {
            Vector3 directionVector = new Vector3(h, v, 0);
            float magnitude = directionVector.magnitude;
            directionVector = directionVector * 100;
            Vector3 playerScreenPos = currentCamera.WorldToScreenPoint(transform.position);

            Vector3 destinationPos;
            if (mainCameraScript != null)
                destinationPos = mainCameraScript.GetPosition(playerScreenPos, directionVector);
            else
                destinationPos = playerScreenPos + directionVector;

            Ray camRay = currentCamera.ScreenPointToRay(destinationPos);
            RaycastHit playerRaycastHit;

            if (Physics.Raycast(camRay, out playerRaycastHit, camRayLength, playerRayCastMask))
            {
                Vector3 playerGoToMax = playerRaycastHit.point - transform.position;
                playerGoToMax.y = 0f;

                Vector3 playerGoTo = playerGoToMax;          
                playerGoTo.Normalize();
                playerGoTo = playerGoTo * magnitude * speed * Time.deltaTime;

                Vector3 newPosition = rigidBody.position + playerGoTo;
                if (playerGoToMax.sqrMagnitude < playerGoTo.sqrMagnitude)
                    newPosition = rigidBody.position + playerGoToMax;           

                rigidBody.MovePosition(newPosition);

                //If we are not aiming, rotate towards direction
                float ah = Input.GetAxisRaw(aimHorizontal);
                float av = Input.GetAxisRaw(aimVertical);
                if (Mathf.Abs(av) < aimThreshold && Mathf.Abs(ah) < aimThreshold)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerGoTo);
                    newRotation = Quaternion.RotateTowards(rigidBody.rotation, newRotation, angularSpeed * Time.deltaTime);
                    rigidBody.MoveRotation(newRotation);
                }
            }
        }
    }

    private void Turn()
    {
        float h = Input.GetAxisRaw(aimHorizontal);
        float v = Input.GetAxisRaw(aimVertical);

        if (Mathf.Abs(v) >= aimThreshold || Mathf.Abs(h) >= aimThreshold)
        {           
            Vector3 directionVector = new Vector3(h, v, 0) * 100;
            Vector3 playerScreenPos = currentCamera.WorldToScreenPoint(transform.position);
            directionVector = playerScreenPos + directionVector;

            Ray camRay = currentCamera.ScreenPointToRay(directionVector);
            RaycastHit playerRaycastHit;

            if (Physics.Raycast(camRay, out playerRaycastHit, camRayLength, playerRayCastMask))
            {
                Vector3 playerLookAt = playerRaycastHit.point - transform.position;
                playerLookAt.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(playerLookAt);
                newRotation = Quaternion.RotateTowards(rigidBody.rotation, newRotation, angularSpeed * Time.deltaTime);
                rigidBody.MoveRotation(newRotation);
            }          
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
