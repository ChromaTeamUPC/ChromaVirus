using UnityEngine;
using System.Collections;

public class jump : MonoBehaviour {
    public float gravity = 0.2f;
    public float upForce = 3;
    public float forwardForce;
    public Transform sphere;
    float currentForce = 0;
    int i;
    Vector3 initPos;

    void Start ()
    {
        initPos = transform.position;
        currentForce = upForce;
        transform.LookAt(sphere);

        // calculo el número de frames que tardará la cabeza en caer de nuevo al suelo
        float y = 0.00001f;
        i = 0;
        float currentForce_ = currentForce;
        while (y >= 0)
        {
            y += currentForce_;
            currentForce_ -= gravity;
            i++;
        }

        // número de pasitos en horizontal que dará la cabeza hasta llegar al objetivo
        forwardForce = Vector3.Distance(initPos, sphere.position) / i;
    }

    void Update ()
    {
        transform.Translate(0, currentForce, 0);    // movimiento vertical
        transform.Translate(Vector3.forward * forwardForce); // movimiento horizontal
        currentForce -= gravity;

        if (transform.position.y < 0)
        {
            transform.position = initPos;
            currentForce = upForce;
            transform.LookAt(sphere);
            forwardForce = Vector3.Distance(initPos, sphere.position) / i;
        }

    }
}
