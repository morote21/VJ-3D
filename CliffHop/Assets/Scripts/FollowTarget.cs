using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float offset_x;
    public float offset_z;
    public float offset_y;
    public Transform target;
    public float forward_displacement;
    public float velocity; // velocidad a la que te moverás en ejes x,z tras pasar su offset (para alcanzar el target)
    public float velocity_y; // velocidad para el eje y (aunque aquí seguiremos último ground)

    float currentv_x;
    float currentv_z;
    float currentv_y;
    Vector3 previousForward; // para activar desplazamiento si ha cambiado
    float lastGround_y; // para seguir eje y

    PlayerController pc; // para tener a mano


    // Start is called before the first frame update
    void Start()
    {
        currentv_x = currentv_z = currentv_y = 0;
        previousForward = target.forward;
        lastGround_y = target.position.y;

        pc = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc.isAlive()) { 
        
            if (previousForward != target.forward)
                currentv_x = currentv_z = velocity;

            Vector3 targetReference = target.position + target.forward*forward_displacement;
            if (pc.isOnGround())
                lastGround_y = targetReference.y; // da igual si de aquí o de target
        

            // Eje x
            if (currentv_x != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetReference.x, transform.position.y, transform.position.z), currentv_x * Time.deltaTime);
                if (transform.position.x == target.position.x)
                    currentv_x = 0;

            }else{
                if (transform.position.x + offset_x <= targetReference.x || transform.position.x - offset_x >= targetReference.x)
                    currentv_x = velocity;
            }

            // Eje z
            if (currentv_z != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, targetReference.z), currentv_z * Time.deltaTime);
                if (transform.position.z == target.position.z)
                    currentv_z = 0;

            }else{
                if (transform.position.z + offset_z <= targetReference.z || transform.position.z - offset_z >= targetReference.z)
                    currentv_z = velocity;
            }

            //Debug.Log("Velocidad x = " + currentv_x);
            //Debug.Log("Velocidad z = " + currentv_z);
            // Eje y (por determinar)
            if (currentv_y != 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, lastGround_y, transform.position.z), currentv_y * Time.deltaTime);
                if (transform.position.y == lastGround_y)
                    currentv_y = 0;

            }
            else
            {
                if (transform.position.y + offset_y <= lastGround_y || transform.position.y - offset_y >= lastGround_y)
                    currentv_y = velocity_y;
            }


            previousForward = target.forward; // actualizar forward previo
        }
    }
}
