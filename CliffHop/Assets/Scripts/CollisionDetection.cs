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
        else if (c.tag == "Coin")
        {
            Debug.Log("Coin collected");
            pc.coinCollected();
            Debug.Log("Number of coins: " + pc.getCoinsCollected().ToString());
            //c.gameObject.SetActive(false);
            Destroy(c.gameObject);
        }
        else if (c.tag == "CornerRight")
        {
            pc.setCanJump(false);
            Debug.Log("Turn right");
            pc.turnDir = 0;
        }
        else
        {
            pc.setCanJump(true);
        }
        
    }
}
