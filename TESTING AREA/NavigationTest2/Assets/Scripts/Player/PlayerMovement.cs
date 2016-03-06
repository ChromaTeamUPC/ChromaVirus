using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10;
    public float aimThreshold = 0.20f;

    private Rigidbody rigidBody;
    private Transform transf;
    public Transform cameraTransform;

	void Awake () {
        rigidBody = GetComponent<Rigidbody>();
        transf = transform;
    }	

    void FixedUpdate()
    {    
        Move();
        Turn();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 displacement = new Vector3(h, 0f, v);
        displacement = cameraTransform.TransformDirection(displacement);
        displacement.y = 0;

        //rigidBody.velocity = displacement * speed;
        
        displacement = displacement * speed * Time.deltaTime; 
        transf.position = transf.position + displacement;    
    }

    void Turn()
    {
        float h = Input.GetAxisRaw("AimHorizontal");
        float v = Input.GetAxisRaw("AimVertical");

        if (Mathf.Abs(v) >= aimThreshold || Mathf.Abs(h) >= aimThreshold)
        {
            Vector3 lookAt = new Vector3(h, 0, v);

            lookAt = cameraTransform.TransformDirection(lookAt);
            transf.rotation = Quaternion.LookRotation(lookAt);
            transf.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }      
    }
}
