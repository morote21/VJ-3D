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
            pc.setCanJump(false);
            Debug.Log("Turn left");
            pc.turnDir = 1;
        }
        else if (c.gameObject.tag == "CornerRight")
        {
            pc.setCanJump(false);
            Debug.Log("Turn right");
            pc.turnDir = 0;
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
            pc.death();
        }
        else if (c.gameObject.tag == "Slow")
        {
            pc.slow(true);
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
    }

    /*
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && (actualCollider.CompareTag("CornerRight") || actualCollider.CompareTag("CornerLeft")))
        {
            actualCollider.gameObject.GetComponent<Renderer>().material.color = Color.green;

            // con esto ajustamos el personaje de manera que cuando giramos un corner, al mismo tiempo lo acercamos un poco al centro del corner para que facilite que el siguiente se haga correctamente
            // pero no se hace de manera exagerada para que no quede raro 
            if (actualCollider.CompareTag("CornerRight"))
            {
                if (pc.gameObject.transform.position.x < actualCollider.transform.position.x)   // en caso de que hayamos girado antes de que los centros sean iguales, se ajusta un poco para eviar problemas
                {
                    pc.GetComponent<CharacterController>().Move(new Vector3((actualCollider.transform.position.x - pc.gameObject.transform.position.x) , 0, 0));
                }
                else if (pc.gameObject.transform.position.x > actualCollider.transform.position.x)
                {
                    pc.GetComponent<CharacterController>().Move(new Vector3(-(pc.gameObject.transform.position.x - actualCollider.transform.position.x) , 0, 0));
                }
            }
            else if (actualCollider.CompareTag("CornerLeft"))
            {
                if (pc.gameObject.transform.position.z < actualCollider.transform.position.z)   // en caso de que hayamos girado despues de que los centros sean iguales (en caso de la z va al reves)
                {
                    pc.GetComponent<CharacterController>().Move(new Vector3(0, 0, -(pc.gameObject.transform.position.z - actualCollider.transform.position.z) ));
                }
                else if (pc.gameObject.transform.position.z > actualCollider.transform.position.z)
                {
                    pc.GetComponent<CharacterController>().Move(new Vector3(0, 0, (actualCollider.transform.position.z - pc.gameObject.transform.position.z) ));
                }
            }
        }
    }
    */
}
