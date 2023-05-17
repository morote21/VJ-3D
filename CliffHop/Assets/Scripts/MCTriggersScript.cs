using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCTriggersScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingCubeZ") || other.CompareTag("MovingCubeX"))
        {
            other.GetComponent<CubeMovement>().changeDirections();
        }
    }
}
