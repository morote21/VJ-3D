using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{

    [SerializeField] float movementSpeed = 3;
    int dir;

    // Start is called before the first frame update
    void Start()
    {
        dir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("MovingCubeZ"))
        {
            transform.Translate(0, 0, dir * movementSpeed * Time.deltaTime, Space.World);
        }       
        else if (gameObject.CompareTag("MovingCubeX"))
        {
            transform.Translate(dir * movementSpeed * Time.deltaTime, 0, 0, Space.World);
        }
    }

    public void changeDirections()
    {
        dir = -dir;
    }
}
