using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController pc;
    
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "CornerLeft")
        {
            pc.setCanJump(false);
            //Debug.Log("Turn left");
            pc.turnDir = 1;
        }
        else if (c.gameObject.tag == "CornerRight")
        {
            pc.setCanJump(false);
            //Debug.Log("Turn right");
            pc.turnDir = 0;
        }
        else if (c.gameObject.tag == "Coin")
        {
            Debug.Log("Coin collected");
            pc.coinCollected();
            Debug.Log("Number of coins: " + pc.getCoinsCollected().ToString());
            //c.gameObject.SetActive(false);
            Destroy(c.gameObject);
        }
        else if (c.gameObject.tag == "Death")
        {
            pc.death();
        }
        else if (c.gameObject.tag == "Slow")
        {
            pc.slow(true);
        }
        else
        {
            // estado normal del jugador (puede saltar y velocidad normal)
            pc.setCanJump(true);
            pc.slow(false);
        }
        
    }


}
