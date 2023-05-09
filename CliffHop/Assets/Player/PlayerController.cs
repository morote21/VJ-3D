using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpHeight;


    //private int score = 0;
    private int coins = 0;

    private float gravity = -50.0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool firstJump;
    private bool canJump = true; // false -> esta en corner, y al darle espacio tiene que girar
                                // true -> no esta en corner y al darle espacio tiene que saltar 
    public int turnDir = 0;     // 0 -> turn right
                                // 1 -> turn left

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        firstJump = false;
        transform.forward = new Vector3(1, 0, 0);   // se inicia mirando hacia la derecha (direccion de las x)
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        } else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        velocity.x = runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            if (!canJump)
            {
                if (turnDir == 1)
                {
                    rotate_player_left();
                } else
                {
                    rotate_player_right();
                }
            } else
            {
                if (isGrounded)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    firstJump = true;
                }
                if (!isGrounded && firstJump)
                {
                    velocity.y = 0;
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    firstJump = false;
                }
            }
        }
        //transform.forward = velocity * Time.deltaTime;
        characterController.Move(new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime);
    }

    public void rotate_player_left()
    {
        transform.Rotate(new Vector3(0f, -90f, 0f));
        canJump = true;
    }

    public void rotate_player_right()
    {
        transform.Rotate(new Vector3(0f, 90f, 0f));
        canJump = true;
    }

    public void setCanJump(bool state)
    {
        canJump = state;
    }

    public void coinCollected()
    {
        coins += 1;
    }

    public int getCoinsCollected()
    {
        return coins;
    }
}
