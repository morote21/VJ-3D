using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawPathingBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    // si saw esta colocado en el eje de las X -> tiene que moverse por el eje de las x y rotar por el eje de las z para la animacion 
    // si saw esta colocado en el eje de las Z -> tiene que moverse por el eje de las z y rotar por el eje de las x para la animacion

    [SerializeField] int dir;    // dir = 0 si esta colocado en el eje de las X
                                 // dir = 1 si esta colocado en el eje de las Z
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;


    void Start()
    {
        if (dir == 0)
        {
            //transform.Rotate(-90, 0, 0);
        }
        else if (dir == 1)
        {
            //transform.Rotate(0, 0, -90);
        }
    }

    // Update is called once per frame
    void Update()
    { 
        if (dir == 0)
        {
            transform.Translate(speed, 0, 0);
        }
        else
        {
            transform.Translate(0, 0, speed);
            //transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SawPath"))
        {
            Debug.Log("Saw Path entered");
        }
        else if (other.CompareTag("SawPathX"))
        {
            Debug.Log("Saw Path X entered");
            dir = 0;
        }
        else if (other.CompareTag("SawPathY"))
        {
            Debug.Log("Saw Path Y entered");
            dir = 1;
        }
        else if (other.CompareTag("Untagged"))
        {
            speed = -speed;
        }
    }

}