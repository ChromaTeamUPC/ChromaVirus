using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {
    public Transform origin1;
    public Transform origin2;

    private Transform target1;
    private Transform target2;
    private PlayerController player1;
    private PlayerController player2;
    private int playerRayCastMask;

    public float smoothing = 5f;

    private Vector3 offset;
    private Camera thisCamera;
    private float cameraBorderMargin = 50f;
    float maxYPosition;
    float maxXPosition;
    float minYPosition;
    float minXPosition;

    void Start()
    {
        playerRayCastMask = LayerMask.GetMask("PlayerRayCast");
        thisCamera = gameObject.GetComponent<Camera>();
        maxYPosition = thisCamera.pixelHeight - cameraBorderMargin;
        maxXPosition = thisCamera.pixelWidth - cameraBorderMargin;
        minYPosition = cameraBorderMargin;
        minXPosition = cameraBorderMargin;
        offset = transform.position - ((origin1.position + origin2.position) / 2);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_SPAWNED, PlayerSpawned);
        mng.eventManager.StartListening(EventManager.EventType.PLAYER_DIED, PlayerDied);
    }

    public void PlayerSpawned(EventInfo eventInfo)
    {
        PlayerController playerController = ((PlayerSpawnedEventInfo)eventInfo).player;

        switch(playerController.Id)
        {
            case 1:
                target1 = playerController.transform;
                player1 = playerController;
                break;

            case 2:
                target2 = playerController.transform;
                player2 = playerController;
                break;
        }
    }

    public void PlayerDied(EventInfo eventInfo)
    {
        PlayerController playerController = ((PlayerSpawnedEventInfo)eventInfo).player;

        switch (playerController.Id)
        {
            case 1: target1 = null; break;
            case 2: target2 = null; break;
        }
    }

    void OnEnable()
    {
        /*if(target1 != null)
            offset = transform.position - target1.position;*/
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (target1 != null && target2 != null)
        {
            Vector3 p1ScreenPos = thisCamera.WorldToScreenPoint(target1.position);
            Vector3 p2ScreenPos = thisCamera.WorldToScreenPoint(target2.position);
            Vector3 targetCamPos = ((p1ScreenPos + p2ScreenPos) / 2);

            Ray camRay = thisCamera.ScreenPointToRay(targetCamPos);
            RaycastHit playerRaycastHit;

            if (Physics.Raycast(camRay, out playerRaycastHit, 100, playerRayCastMask))
            {
                targetCamPos = playerRaycastHit.point + offset;

                transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            }
        }
        else if (target1 != null)
        {
            Vector3 targetCamPos = target1.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
        else if (target2 != null)
        {
            Vector3 targetCamPos = target2.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }

    public Vector3 GetPosition(Vector3 originalPosition, Vector3 displacement)
    {
        if (target1 != null && target2 != null)
        {
            Vector3 p1ScreenPos = thisCamera.WorldToScreenPoint(target1.position);
            Vector3 p2ScreenPos = thisCamera.WorldToScreenPoint(target2.position);

            float yMargin = Mathf.Min(maxYPosition - p1ScreenPos.y, maxYPosition - p2ScreenPos.y) +
                Mathf.Min(p1ScreenPos.y - minYPosition, p2ScreenPos.y - minYPosition);

            float xMargin = Mathf.Min(maxXPosition - p1ScreenPos.x, maxXPosition - p2ScreenPos.x) +
                Mathf.Min(p1ScreenPos.x - minXPosition, p2ScreenPos.x - minXPosition);


            Vector3 finalPosition = originalPosition + displacement;

            if (finalPosition.y < minYPosition)
            {
                finalPosition.y = Mathf.Max(finalPosition.y, originalPosition.y - yMargin);
            }
            else if (finalPosition.y > maxYPosition)
            {
                finalPosition.y = Mathf.Min(finalPosition.y, originalPosition.y + yMargin);
            }

            if (finalPosition.x < minXPosition)
            {
                finalPosition.x = Mathf.Max(finalPosition.x, originalPosition.x - xMargin);
            }
            else if (finalPosition.x > maxXPosition)
            {
                finalPosition.x = Mathf.Min(finalPosition.x, originalPosition.x + xMargin);
            }

            return finalPosition;
        }
        else
        {
            return originalPosition + displacement;
        }
    }
}
