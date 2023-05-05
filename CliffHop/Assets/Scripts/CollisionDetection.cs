using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController pc;
    void OnTriggerExit(Collider c)
    {
        
    }

    void OnTriggerStay(Collider c)
    {
        if (c.tag == "CornerLeft")
        {
            pc.canJump = false;
            Debug.Log("Turn left");
            //Input.GetButtonDown("Jump") && 
            if (Input.GetButtonDown("Jump") && pc.prevDir == 0)
            {
                pc.rotate_player_left();
            }
        }

        if (c.tag == "CornerRight")
        {
            pc.canJump = false;
            Debug.Log("Turn right");
            if (Input.GetButtonDown("Jump") && pc.prevDir == 1)
            {
                pc.rotate_player_right();
            }
        }
        if (c.tag != "CornerLeft" && c.tag != "CornerRight")
        {
            pc.canJump = true;
        }
        
    }
}
