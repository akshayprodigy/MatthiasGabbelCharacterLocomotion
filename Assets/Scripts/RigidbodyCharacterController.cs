using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCharacterController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 7f;
    public float acceleration = 1f;
    public float crouchSpeed = 2.5f;
    public float crouchHeightRatio = 0.5f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded;
    private float originalHeight;
    private float currentSpeed;
    private bool isCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Determine the target speed based on input
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Gradually interpolate the current speed towards the target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // Handle Movement Input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * currentSpeed;
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement) * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the character is grounded
        if (collision.contacts[0].normal == Vector3.up)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
