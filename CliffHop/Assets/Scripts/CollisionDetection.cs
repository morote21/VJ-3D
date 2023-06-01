using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController pc;
    private Collider actualCollider;

    [SerializeField] private AudioSource coinSoundEffect;
    [SerializeField] private AudioSource screamSoundEffect;
    [SerializeField] private AudioSource slowSoundEffect;



    void OnTriggerEnter(Collider c)
    {
        
        if (c.gameObject.tag == "Coin")
        {
            GameManager.instance.coinPickup();
            //c.gameObject.SetActive(false);
            Destroy(c.gameObject);
            coinSoundEffect.Play();
        }
        else if (c.gameObject.tag == "Death")
        {
            if (!pc.isGodMode() && pc.isAlive())
            {
                pc.death(c);
                GameManager.instance.gameMusic.Stop();
                screamSoundEffect.Play();
            }
        }
        else if (c.gameObject.tag == "Slow")
        {
            if (!pc.isGodMode())
            {
                pc.slow(true);
                slowSoundEffect.Play();
            }
        }
        else if (c.gameObject.tag == "Win")
        {
            pc.victory();
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
                    //actualCollider = other;
                }
            }
        }
        else if (other.CompareTag("Jump"))
        {
            pc.setJumpCollider(other);
            if (pc.isGodMode())
            {
                pc.jumpInGodMode();
            }
        }
        else if (other.CompareTag("DoubleJump"))
        {
            pc.setJumpCollider(other);
            if (pc.isGodMode())
            {
                pc.doubleJumpInGodMode();
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
