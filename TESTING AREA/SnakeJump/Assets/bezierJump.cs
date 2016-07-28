using UnityEngine;
using System.Collections;

public class bezierJump : MonoBehaviour {
    public Transform esfera;
    public int altura = 20;
    public float duracion = 1;

    Vector3 start;
    Vector3 control;
    Vector3 end;

    Vector3 lastPosition;

    float t = 0;

    void Start ()
    {
        start = transform.position;
        end = esfera.position;

        control = (end - start) / 2;
        control.y = altura;

        lastPosition = start;
	}
	
	void Update ()
    {
        // si ha hecho el salto reinicia todo para dar otro
        if (t >= 1)
        {
            t = 0;
            end = esfera.position;

            control = (end - start) / 2;
            control.y = altura;

            lastPosition = start;
        }

        transform.position = Bezier(start, control, end, t);
        t += (1 / duracion) * Time.deltaTime;
        transform.LookAt(transform.position + (transform.position - lastPosition));

        lastPosition = transform.position;
	}

    Vector3 Bezier(Vector3 start_, Vector3 control_, Vector3 end_, float t_)
    {
        return (((1 - t_) * (1 - t_)) * start_) + (2 * t_ * (1 - t_) * control_) + ((t_ * t_) * end_);
    }
}

