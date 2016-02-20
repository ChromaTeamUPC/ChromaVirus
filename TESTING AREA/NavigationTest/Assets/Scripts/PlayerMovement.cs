using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    public float aimThreshold;
    public GameObject test;
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
        //Animating(h, v);
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 displacement = new Vector3(h, 0f, v);
        displacement = displacement * speed * Time.deltaTime;
        displacement = cameraTransform.TransformDirection(displacement);
        displacement.y = 0;
        transform.position = transform.position + displacement;
    }

    void Turn()
    {
        float h = Input.GetAxisRaw("AimHorizontal");
        float v = Input.GetAxisRaw("AimVertical");

        if (Mathf.Abs(v) >= aimThreshold || Mathf.Abs(h) >= aimThreshold)
        {
            Vector3 lookAt = new Vector3(h, 0, v);
            lookAt.x += (h * 10);
            lookAt.z += (v * 10);

            test.transform.position = transf.position + lookAt;
            /*Vector3 sphereNewPos = transf.position + lookAt;
            sphereNewPos.y = test.transform.position.y;
            test.transform.position = sphereNewPos;*/

            lookAt = cameraTransform.TransformDirection(lookAt);

            Quaternion newRotation = Quaternion.LookRotation(lookAt);
            //rigidBody.MoveRotation(newRotation);
            transf.rotation = newRotation;
            transf.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            /*Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            direction = cameraTransform.TransformDirection(direction);
            transform.rotation = Quaternion.LookRotation(direction);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);*/
        }      
    }
}
