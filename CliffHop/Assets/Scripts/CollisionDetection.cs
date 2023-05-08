using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController pc;
    
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "CornerLeft")
        {
            pc.setCanJump(false);
            Debug.Log("Turn left");
            pc.turnDir = 1;
        }

        if (c.tag == "CornerRight")
        {
            pc.setCanJump(false);
            Debug.Log("Turn right");
            pc.turnDir = 0;
        }
        if (c.tag != "CornerLeft" && c.tag != "CornerRight")
        {
            pc.setCanJump(true);
        }
        
    }
}
