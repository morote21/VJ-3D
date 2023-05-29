using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController pc;
    private Collider actualCollider;

    [SerializeField] private AudioSource coinSoundEffect;


    void OnTriggerEnter(Collider c)
    {
        /*
        if (c.gameObject.tag == "CornerLeft")
        {
            Debug.Log("Turn left");
        }
        else if (c.gameObject.tag == "CornerRight")
        {
            Debug.Log("Turn right");
        }
        */
        
        if (c.gameObject.tag == "Coin")
        {
            Debug.Log("Coin collected");
            pc.coinCollected();
            Debug.Log("Number of coins: " + pc.getCoinsCollected().ToString());
            //c.gameObject.SetActive(false);
            Destroy(c.gameObject);
            coinSoundEffect.Play();
        }
        else if (c.gameObject.tag == "Death")
        {
            if (!pc.isGodMode())
            {
                pc.death();
            }
        }
        else if (c.gameObject.tag == "Slow")
        {
            if (!pc.isGodMode())
            {
                pc.slow(true);
            }
        }
        else if (c.gameObject.tag == "Win")
        {
            pc.victory();
        }
        else if (c.gameObject.tag == "Underwater")
        {
            if (pc.isGodMode())
                pc.respawnToLastCorner();
            else
                pc.death();
        }
        else
        {
            // estado normal del jugador (puede saltar y velocidad normal)
            //pc.setCanJump(true);
            pc.slow(false);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CornerLeft"))
        {
            if (pc.getPlayerCollider().bounds.Contains(other.bounds.center))
            {
                if (pc.turnDir == 0)
                {
                    pc.setCanJump(false);
                    pc.turnDir = 1;
                    pc.setCollider(other);
                    //actualCollider = other;
                    Debug.Log("turn left!");
                }
            }
        }
        else if (other.CompareTag("CornerRight"))
        {
            if (pc.getPlayerCollider().bounds.Contains(other.bounds.center))
            {
                if (pc.turnDir == 1)
                {
                    pc.setCanJump(false);
                    pc.turnDir = 0;
                    pc.setCollider(other);
                    Debug.Log("turn right!");
                    //actualCollider = other;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CornerLeft"))
        {
            pc.setCanJump(true);
        }
        else if (other.CompareTag("CornerRight"))
        {
            pc.setCanJump(true);
        }
    }

}
