using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float angularSpeed = 360;
    public float aimThreshold = 0.2f;

    public Transform cameraTransform;
    private Rigidbody rigidBody;

	void Awake () {
        rigidBody = GetComponent<Rigidbody>();
    }	

    void Start()
    {
        mng.eventManager.StartListening(EventManager.EventType.CAMERA_CHANGED, CameraChanged);
    }

    void CameraChanged(EventInfo eventInfo)
    {
        CameraEventInfo info = (CameraEventInfo)eventInfo;
        cameraTransform = info.newCamera.transform;
    }

    void FixedUpdate()
    {    
        Move();
        Turn();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("P1_Horizontal");
        float v = Input.GetAxisRaw("P1_Vertical");

        Vector3 displacement = new Vector3(h, 0f, v);

        //Get the Y rotation angle from the camera
        float camRotation = cameraTransform.rotation.eulerAngles.y;
        //Apply that rotation to the direction vector
        displacement = Quaternion.Euler(0, camRotation, 0) * displacement;

        //Add velocity and apply it
        displacement = displacement * speed * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + displacement);   
    }

    void Turn()
    {
        float h = Input.GetAxisRaw("P1_AimHorizontal");
        float v = Input.GetAxisRaw("P1_AimVertical");

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
}
