using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float normalRunSpeed;
    [SerializeField] private float slowRunSpeed;
    [SerializeField] private float jumpHeight;

    //private int score = 0;
    private int coins = 0;

    private float speedMovement;
    private float gravity = -50.0f;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool firstJump;
    private bool canJump = true; // false -> esta en corner, y al darle espacio tiene que girar
                                // true -> no esta en corner y al darle espacio tiene que saltar 
    public int turnDir = 0;     // 0 -> turn right
                                // 1 -> turn left

    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();  // in children, ya que es el ch09 quien tiene el animator (el cual es el hijo de la clase Player)
        firstJump = false;
        transform.forward = new Vector3(1, 0, 0);   // se inicia mirando hacia la derecha (direccion de las x)
        speedMovement = normalRunSpeed;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0;
            } else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            velocity.x = speedMovement;

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
            Vector3 newPosition = new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime;
            characterController.Move(newPosition);

            animator.SetFloat("Speed", velocity.x);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", velocity.y);
            animator.SetBool("FirstJump", firstJump);
        }
        animator.SetBool("Dead", dead);
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

    public void death()
    {
        Debug.Log("Dead!");
        //characterController.Move(-transform.position);
        dead = true;
    }

    public void alive()
    {
        dead = false;
    }

    public void slow(bool slow)
    {
        if (slow)
        {
            speedMovement = slowRunSpeed;
        }
        else
        {
            speedMovement = normalRunSpeed;
        }
    }
}
