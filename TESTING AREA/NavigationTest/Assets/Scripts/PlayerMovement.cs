using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float maxRotation;
    public float maxAcceleration;
    private float lastDisplacementLength;
    private float lastDisplacementLengthSqr;
    public float aimThreshold;
    
    public GameObject test;
    private Rigidbody rigidBody;
    private Transform transf;
    public Transform cameraTransform;

	void Awake () {
        rigidBody = GetComponent<Rigidbody>();
        transf = transform;
        lastDisplacementLength = 0.0f;
        lastDisplacementLengthSqr = 0.0f;
    }	

    void FixedUpdate()
    {    
        Move();
        Turn();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 displacement = new Vector3(h, 0f, v);

        displacement = displacement * speed * Time.deltaTime;
        displacement = cameraTransform.TransformDirection(displacement);
        displacement.y = 0;
        transf.position = transf.position + displacement;     
    }

    void Turn()
    {
        float h = Input.GetAxisRaw("AimHorizontal");
        float v = Input.GetAxisRaw("AimVertical");

        if (Mathf.Abs(v) >= aimThreshold || Mathf.Abs(h) >= aimThreshold)
        {
            Vector3 lookAt = new Vector3(h, 0, v);
            //lookAt.x += (h * 10);
            //lookAt.z += (v * 10);
            //test.transform.position = transf.position + lookAt;

            lookAt = cameraTransform.TransformDirection(lookAt);
            transf.rotation = Quaternion.LookRotation(lookAt);
            transf.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }      
    }
}
