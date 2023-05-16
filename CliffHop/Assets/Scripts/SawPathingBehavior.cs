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
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (dir == 0)
        {
            // por defecto los rotates y translates se hacen en local space (space.self)
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
            transform.Translate(speed, 0, 0, Space.World);
        }
        else
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
            transform.Translate(0, 0, speed, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SawPath"))
        {
            //Debug.Log("Saw Path entered");
        }
        else if (other.CompareTag("SawPathX"))
        {
            dir = 0;
        }
        else if (other.CompareTag("SawPathZ"))
        {
            dir = 1;
        }
        else if (other.CompareTag("Untagged"))
        {
            speed = -speed;
        }
    }

}
