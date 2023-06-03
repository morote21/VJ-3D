using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float normalRunSpeed;
    [SerializeField] private float jumpHeight;

    private Collider actualCollider;
    private Collider jumpCollider;
    private float speedMovement, gravity = -50.0f, currentTime, djumpTime, respawnTimer, respawnTime;
    private CharacterController characterController;
    private Transform playerPosition;
    private Animator animator;
    private Vector3 velocity, lastCornerPosition;
    private bool isGrounded, secondJump, isRunning;
    private bool canJump; // false -> esta en corner, y al darle espacio tiene que girar
                          // true -> no esta en corner y al darle espacio tiene que saltar 
    public int turnDir = 0;     // 0 -> turn right
                                // 1 -> turn left

    private bool jumping, win, stop, lastAnimationPlayed, respawned;
    private int corners;

    public PlayerController pc;

    public ParticleSystem runVFX;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();  // in children, ya que es el ch09 quien tiene el animator (el cual es el hijo de la clase Player)
        secondJump = jumping = win = stop = lastAnimationPlayed = false;
        canJump = true;
        transform.forward = new Vector3(1, 0, 0);   // se inicia mirando hacia la derecha (direccion de las x)
        speedMovement = normalRunSpeed;
        lastCornerPosition = new Vector3(2, 1.01f, 0);
        currentTime = respawnTime = 0f;
        djumpTime = 0.55f;
        respawnTimer = 1f;
        corners = 0;
        respawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pc.hasWon() && pc.isAlive())
        {
            if (respawned)      // en cuanto el personaje reaparece, tambien lo hace el zombie y espera un rato para no ir en la misma posicion que el jugador
            {
                respawnTime += Time.deltaTime;
                stopMovement();
                if (respawnTime >= respawnTimer)
                {
                    respawned = false;
                    resetMovement();
                    respawnTime = 0f;
                }
            }
            else
            {
                if (isRunning)  // timer para el segundo salto 
                {
                    currentTime += Time.deltaTime;
                    if (currentTime >= djumpTime)
                    {
                        if (jumping && !isGrounded && secondJump)
                        {
                            velocity.y = 0;
                            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                            animator.Play("DoubleJump");
                            secondJump = false;
                            //Debug.Log("Second jump");
                            //jumpSoundEffect.Play();
                            stopTimer();
                            if (runVFX.isPlaying)
                            {
                                runVFX.Stop();
                            }
                        }
                    }
                }

                if (!win)
                {
                    isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
                    if (isGrounded && velocity.y < 0 && velocity.x > 0)
                    {
                        velocity.y = 0;
                        jumping = false;
                        secondJump = false;
                        animator.Play("Running");
                        if (!runVFX.isPlaying)
                        {
                            runVFX.Play();
                        }
                    }
                    else
                    {
                        velocity.y += gravity * Time.deltaTime;
                        if (runVFX.isPlaying)
                        {
                            runVFX.Stop();
                        }
                    }

                    velocity.x = speedMovement;

                    if (!canJump)
                    {
                        if (turnDir == 1)   // corner left
                        {
                            if (gameObject.transform.position.z < actualCollider.transform.position.z)
                            {
                                rotate_player_left();
                                //turnSoundEffect.Play();
                                lastCornerPosition = actualCollider.transform.position;
                                characterController.Move(new Vector3(0, 0, -(gameObject.transform.position.z - actualCollider.transform.position.z)));
                            }
                        }
                        else // corner right
                        {
                            if (gameObject.transform.position.x > actualCollider.transform.position.x)
                            {
                                rotate_player_right();
                                //turnSoundEffect.Play();
                                lastCornerPosition = actualCollider.transform.position;
                                characterController.Move(new Vector3(-(gameObject.transform.position.x - actualCollider.transform.position.x), 0, 0));
                            }

                        }
                    }

                    Vector3 newPosition = new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime;
                    characterController.Move(newPosition);

                }
            }
            animator.SetFloat("Speed", velocity.x);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", velocity.y);
            animator.SetBool("SecondJump", secondJump);
        }
        else
        {
            if (!isGrounded)
            {
                isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);

                velocity.y += gravity * Time.deltaTime;
                velocity.x = speedMovement;
                Vector3 newPosition = new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime;
                characterController.Move(newPosition);

                animator.SetFloat("Speed", velocity.x);
                animator.SetBool("IsGrounded", isGrounded);
                animator.SetFloat("VerticalSpeed", velocity.y);
                animator.SetBool("SecondJump", secondJump);
            }
            else
            {
                if (!lastAnimationPlayed && pc.hasWon())
                {
                    animator.Play("Death");
                }
                else if (!lastAnimationPlayed && !pc.isAlive())
                {
                    animator.Play("Victory Idle");
                }
                lastAnimationPlayed = true;
            }
        }
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

    public Collider getPlayerCollider()
    {
        return GetComponent<BoxCollider>();
    }

    public void setCollider(Collider c)
    {
        actualCollider = c;
    }

    public void setJumpCollider(Collider c)
    {
        jumpCollider = c;
    }
    public void respawnToLastCorner()
    {
        //characterController.Move(lastCornerPosition - gameObject.transform.position);

        characterController.enabled = false;
        transform.position = lastCornerPosition;
        characterController.enabled = true;
        animator.Play("Zombie Idle");
        respawned = true;

        canJump = true;

        // a causa de la condicion de turnDir en el triggerstay, nada mas entrar ya cambia el valor, aunque no se le haya dado a la tecla espacio
        // y de normal al respawnear, como se respawnea en el corner anterior entonces se restablece menos en el origen, que al reaparecer en el 0,0,0 y no en un corner el valor cambia
        // y no se vuelve a restablecer
        if (corners == 0)
            turnDir = 1;

    }

    public void jumpInGodMode()
    {
        if (turnDir == 1)   // salto en el eje de las x
        {
            if (gameObject.transform.position.x > jumpCollider.transform.position.x)
            {
                if (!jumping)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    secondJump = true;
                    animator.Play("Jump");
                    jumping = true;
                    //jumpSoundEffect.Play();
                }
            }
        }
        else
        {
            if (gameObject.transform.position.z < jumpCollider.transform.position.z)
            {
                if (!jumping)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    secondJump = true;
                    animator.Play("Jump");
                    jumping = true;
                    //jumpSoundEffect.Play();
                }
            }
        }
    }

    public void doubleJumpInGodMode()
    {
        if (turnDir == 1)   // salto en el eje de las x
        {
            if (gameObject.transform.position.x > jumpCollider.transform.position.x)
            {
                if (!jumping)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    secondJump = true;
                    animator.Play("Jump");
                    jumping = true;
                    //jumpSoundEffect.Play();
                    startTimer();
                }
            }
        }
        else
        {
            if (gameObject.transform.position.z < jumpCollider.transform.position.z)
            {
                if (!jumping)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
                    secondJump = true;
                    animator.Play("Jump");
                    jumping = true;
                    //jumpSoundEffect.Play();
                    startTimer();
                }
            }
        }
    }

    public void startTimer()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void stopTimer()
    {
        isRunning = false;
    }

    public void stopMovement()
    {
        speedMovement = 0;
        velocity.x = velocity.y = 0;
        stop = true;
        animator.Play("Zombie Idle");
        if (runVFX.isPlaying)
        {
            runVFX.Stop();
        }
    }

    public void resetMovement()
    {
        speedMovement = normalRunSpeed;
        animator.Play("Running");
        if (!runVFX.isPlaying)
        {
            runVFX.Play();
        }
    }

}
