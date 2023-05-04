using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayers;
    [SerializeField] private float runSpeed;

    private float gravity = -50.0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    //private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayers, QueryTriggerInteraction.Ignore);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        } else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        velocity.x = runSpeed;
        //characterController.Move(new Vector3(runSpeed, 0, 0) * Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);
    }
}
