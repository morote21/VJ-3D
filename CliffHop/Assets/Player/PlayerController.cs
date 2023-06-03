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
    [SerializeField] private AudioSource turnSoundEffect;
    [SerializeField] private AudioSource doubleJumpSoundEffect;

    private Collider actualCollider;
    private Collider jumpCollider;

    //private int score = 0;
    private int corners = 0;

    private float speedMovement, gravity = -50.0f, currentTime, djumpTime, defeatTimer, victoryTimer;
    private CharacterController characterController;
    private Transform playerPosition;
    private Animator animator;
    private Vector3 velocity, lastCornerPosition;
    private bool isGrounded, secondJump, isRunning;
    private bool canJump; // false -> esta en corner, y al darle espacio tiene que girar
                          // true -> no esta en corner y al darle espacio tiene que saltar 
    public int turnDir = 0;     // 0 -> turn right
                                // 1 -> turn left

    private bool dead, jumping, win, godmode;

    public Material cornerPressedMaterial;
    public ParticleSystem hitVFX;
    public ParticleSystem runVFX;

    public DefeatMenu dm;
    public VictoryMenu vm;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();  // in children, ya que es el ch09 quien tiene el animator (el cual es el hijo de la clase Player)
        secondJump = dead = jumping = win = godmode = false;
        canJump = true;
        transform.forward = new Vector3(1, 0, 0);   // se inicia mirando hacia la derecha (direccion de las x)
        speedMovement = normalRunSpeed;
        lastCornerPosition = new Vector3(2, 1.01f, 0);
        currentTime = defeatTimer = victoryTimer = 0f;
        djumpTime = 0.55f;
        animator.Play("Breathing Idle");
        velocity.x = velocity.y = 0f;
    }

    // Update is called once per frame
    void Update()
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
                    doubleJumpSoundEffect.Play();
                    stopTimer();
                    if (runVFX.isPlaying)
                    {
                        runVFX.Stop();
                    }
                }
            }

        }

        if (!dead && !win)
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
                if (runVFX.isPlaying)
                {
                    runVFX.Stop();
                }
                velocity.y += gravity * Time.deltaTime;
            }

            if (!isGrounded && !jumping && !godmode)
            {
                if (runVFX.isPlaying)
                {
                    runVFX.Stop();
                }
                animator.Play("Falling");
            }

            velocity.x = speedMovement;

            if (Input.GetKeyDown(KeyCode.G))
            {
                godmode = !godmode;
                if (godmode)
                    Debug.Log("God mode activated");
                else
                    Debug.Log("God mode deactivated");
            }

            // en caso de que el godmode este activado entonces los saltos los controla la maquina

            if (godmode)
            {
                if (!canJump)
                {
                    if (turnDir == 1)   // corner left
                    {
                        if (gameObject.transform.position.z < actualCollider.transform.position.z)
                        {
                            actualCollider.gameObject.GetComponentInChildren<Renderer>().material = cornerPressedMaterial;
                            rotate_player_left();
                            turnSoundEffect.Play();
                            lastCornerPosition = actualCollider.transform.position;
                            characterController.Move(new Vector3(0, 0, -(gameObject.transform.position.z - actualCollider.transform.position.z)));  
                        }
                    }
                    else // corner right
                    {
                        if (gameObject.transform.position.x > actualCollider.transform.position.x)
                        {
                            actualCollider.gameObject.GetComponentInChildren<Renderer>().material = cornerPressedMaterial;
                            rotate_player_right();
                            turnSoundEffect.Play();
                            lastCornerPosition = actualCollider.transform.position;
                            characterController.Move(new Vector3(-(gameObject.transform.position.x - actualCollider.transform.position.x), 0, 0));
                        }

                    }
                }
                else
                {

                }
            }

            else
            {
                if (!GameManager.instance.isGamePaused())
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        if (!canJump)
                        {
                            if (turnDir == 1)
                            {
                                rotate_player_left();

                                turnSoundEffect.Play();

                                //actualCollider.gameObject.GetComponent<Renderer>().material.color = Color.green;
                                actualCollider.gameObject.GetComponentInChildren<Renderer>().material = cornerPressedMaterial;

                                if (gameObject.transform.position.z < actualCollider.transform.position.z)   // en caso de que hayamos girado despues de que los centros sean iguales (en caso de la z va al reves)
                                {
                                    lastCornerPosition = actualCollider.transform.position;
                                    characterController.Move(new Vector3(0, 0, -(gameObject.transform.position.z - actualCollider.transform.position.z)));
                                }
                                else if (gameObject.transform.position.z > actualCollider.transform.position.z)
                                {
                                    lastCornerPosition = actualCollider.transform.position;
                                    characterController.Move(new Vector3(0, 0, (actualCollider.transform.position.z - gameObject.transform.position.z)));
                                }
                            }
                            else
                            {
                                rotate_player_right();

                                turnSoundEffect.Play();

                                //actualCollider.gameObject.GetComponent<Renderer>().material.color = Color.green;
                                actualCollider.gameObject.GetComponentInChildren<Renderer>().material = cornerPressedMaterial;

                                if (gameObject.transform.position.x < actualCollider.transform.position.x)   // en caso de que hayamos girado antes de que los centros sean iguales, se ajusta un poco para eviar problemas
                                {
                                    lastCornerPosition = actualCollider.transform.position;
                                    characterController.Move(new Vector3((actualCollider.transform.position.x - gameObject.transform.position.x), 0, 0));
                                }
                                else if (gameObject.transform.position.x > actualCollider.transform.position.x)
                                {
                                    lastCornerPosition = actualCollider.transform.position;
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
                                doubleJumpSoundEffect.Play();
                            }
                        }
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

            if (win)
            {
                victoryTimer += Time.deltaTime;
                if (victoryTimer >= 3f)
                {
                    victoryTimer = 0f;
                    vm.activate();
                }
            }
            else
            {
                defeatTimer += Time.deltaTime;
                if (defeatTimer >= 2f)
                {
                    defeatTimer = 0f;
                    dm.activate();
                }
            }



        }
        //Debug.Log(lastCornerPosition);
        //Debug.Log(canJump);
        animator.SetBool("Dead", dead);
    }

    public void rotate_player_left()
    {
        transform.Rotate(new Vector3(0f, -90f, 0f));
        canJump = true;
        corners += 1;
        GameManager.instance.setHighScore(corners);   // si es highscore o no se comprueba dentro de gm
    }


    public void rotate_player_right()
    {
        transform.Rotate(new Vector3(0f, 90f, 0f));
        canJump = true;
        corners += 1;
        GameManager.instance.setHighScore(corners);   // si es highscore o no se comprueba dentro de gm
    }

    public void setCanJump(bool state)
    {
        canJump = state;
    }


    public int getCornersTurned()
    {
        return corners;
    }

    public void death(Collider c)
    {
        if (!dead)
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        dead = true;
        //animator.Play("Falling");
        animator.Play("Knocked Down");

        if (c.gameObject.name.Contains("Cannon"))
        {
            hitVFX.Play();
        }
        else if (c.gameObject.name.Contains("Spike"))
        {
            //Debug.Log("Spikes");
            // sonido pinchos
        }
        else if (c.gameObject.name.Contains("Saw"))
        {
            //Debug.Log("Saw");
            // sonido sierra
        }

        if (runVFX.isPlaying)
        {
            runVFX.Stop();
        }
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

    public void setJumpCollider(Collider c)
    {
        jumpCollider = c;
    }

    public bool isOnGround()
    { 
        return isGrounded && !jumping;
    }

    public bool isAlive()
    {
        return !dead;
    }

    public void victory()
    {
        win = true;
        velocity.x = velocity.y = 0;
        gravity = 0;
        animator.Play("Victory Idle");
        GameManager.instance.stopMusic();

        if (runVFX.isPlaying)
        {
            runVFX.Stop();
        }
    }

    public bool isGodMode()
    {
        return godmode;
    }

    public void respawnToLastCorner()
    {
        //characterController.Move(lastCornerPosition - gameObject.transform.position);

        characterController.enabled = false;
        transform.position = lastCornerPosition;
        characterController.enabled = true;

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
                    jumpSoundEffect.Play();
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
                    jumpSoundEffect.Play();
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
                    jumpSoundEffect.Play();
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
                    jumpSoundEffect.Play();
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

    public bool hasWon()
    {
        return win;
    }

}
