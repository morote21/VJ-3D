using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float normalRunSpeed;
    [SerializeField] private float slowRunSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private AudioSource jumpSoundEffect;

    private Collider actualCollider;

    //private int score = 0;
    private int coins = 0, corners = 0;

    private float speedMovement, gravity = -50.0f;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded, secondJump;
    private bool canJump; // false -> esta en corner, y al darle espacio tiene que girar
                          // true -> no esta en corner y al darle espacio tiene que saltar 
    public int turnDir = 0;     // 0 -> turn right
                                // 1 -> turn left

    private bool dead, jumping;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();  // in children, ya que es el ch09 quien tiene el animator (el cual es el hijo de la clase Player)
        secondJump = dead = jumping = false;
        canJump = true;
        transform.forward = new Vector3(1, 0, 0);   // se inicia mirando hacia la derecha (direccion de las x)
        speedMovement = normalRunSpeed;
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
                jumping = false;
                animator.Play("Running");
            } 
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            if (!isGrounded && !jumping)
            {
                animator.Play("Falling");
            }

            velocity.x = speedMovement;

            if (Input.GetButtonDown("Jump"))
            {
                if (!canJump)
                {
                    if (turnDir == 1)
                    {
                        rotate_player_left();

                        actualCollider.gameObject.GetComponent<Renderer>().material.color = Color.green;

                        if (gameObject.transform.position.z < actualCollider.transform.position.z)   // en caso de que hayamos girado despues de que los centros sean iguales (en caso de la z va al reves)
                        {
                            characterController.Move(new Vector3(0, 0, -(gameObject.transform.position.z - actualCollider.transform.position.z)));
                        }
                        else if (gameObject.transform.position.z > actualCollider.transform.position.z)
                        {
                            characterController.Move(new Vector3(0, 0, (actualCollider.transform.position.z - gameObject.transform.position.z)));
                        }
                    }
                    else
                    {
                        rotate_player_right();

                        actualCollider.gameObject.GetComponent<Renderer>().material.color = Color.green;

                        if (gameObject.transform.position.x < actualCollider.transform.position.x)   // en caso de que hayamos girado antes de que los centros sean iguales, se ajusta un poco para eviar problemas
                        {
                            characterController.Move(new Vector3((actualCollider.transform.position.x - gameObject.transform.position.x), 0, 0));
                        }
                        else if (gameObject.transform.position.x > actualCollider.transform.position.x)
                        {
                            characterController.Move(new Vector3(-(gameObject.transform.position.x - actualCollider.transform.position.x), 0, 0));
                        }
                    }
                    
                } else
                {
                    
                    if (!jumping)
                    {
                        velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                        secondJump = true;
                        animator.Play("Jump");
                        jumping = true;
                        jumpSoundEffect.Play();
                    }
                    else if (jumping && !isGrounded && secondJump)
                    {
                        velocity.y = 0;
                        velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                        animator.Play("DoubleJump");
                        secondJump = false;
                        //Debug.Log("Second jump");
                        jumpSoundEffect.Play();
                    }
                }
            }
            Vector3 newPosition = new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime;
            characterController.Move(newPosition);

            animator.SetFloat("Speed", velocity.x);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", velocity.y);
            animator.SetBool("SecondJump", secondJump);
        }

        else
        {
            Vector3 newPosition = new Vector3(-transform.forward.z * velocity.x, velocity.y, transform.forward.x * velocity.x) * Time.deltaTime;
            characterController.Move(newPosition);

            velocity.y += gravity * Time.deltaTime;
        }

        animator.SetBool("Dead", dead);
    }

    public void rotate_player_left()
    {
        transform.Rotate(new Vector3(0f, -90f, 0f));
        canJump = true;
        corners += 1;
    }


    public void rotate_player_right()
    {
        transform.Rotate(new Vector3(0f, 90f, 0f));
        canJump = true;
        corners += 1;
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

    public int getCornersTurned()
    {
        return corners;
    }

    public void death()
    {
        Debug.Log("Dead!");
        velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        dead = true;
        animator.Play("Falling");
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

    public Collider getPlayerCollider()
    {
        return GetComponent<BoxCollider>();
    }

    public void setCollider(Collider c)
    {
        actualCollider = c;
    }

    public bool isOnGround()
    { 
        return isGrounded && !jumping;
    }

    public bool isAlive()
    {
        return !dead;
    }

}
