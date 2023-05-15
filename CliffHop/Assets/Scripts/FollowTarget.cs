using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float offset_x;
    public float offset_z;
    public Transform target;
    public float forward_displacement;
    public float velocity; // velocidad a la que te moverás en cada eje tras pasar su offset (para alcanzar el target)

    float currentv_x;
    float currentv_z;
    Vector3 previousForward; // para activar desplazamiento si ha cambiado


    // Start is called before the first frame update
    void Start()
    {
        currentv_x = currentv_z = 0;
        previousForward = target.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousForward != target.forward)
            currentv_x = currentv_z = velocity;

        Vector3 targetReference = target.position + target.forward*forward_displacement; 

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
        // ...

        previousForward = target.forward; // actualizar forward previo
    }
}
