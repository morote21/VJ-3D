using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpHeight;

    private float gravity = -50.0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool firstJump;
    public bool canJump = true;
    public int prevDir = 1;     // 0 -> direccion previa hacia la derecha, tendra que girar hacia la izquierda
                                // 1 -> direccion previa hacia la izquierda, tendra que girar hacia la derechas

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

        if (isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            firstJump = true;
        }
        if (!isGrounded && firstJump && canJump && Input.GetButtonDown("Jump"))
        {
            velocity.y = 0;
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            firstJump = false;
        }
        //transform.forward = velocity * Time.deltaTime;
        characterController.Move(new Vector3(transform.forward.x * velocity.x, velocity.y, transform.forward.z * velocity.x) * Time.deltaTime);
    }

    public void rotate_player_left()
    {
        transform.Rotate(new Vector3(0f, -90f, 0f));
        prevDir = 1;
    }

    public void rotate_player_right()
    {
        transform.Rotate(new Vector3(0f, 90f, 0f));
        prevDir = 0;
    }


}
