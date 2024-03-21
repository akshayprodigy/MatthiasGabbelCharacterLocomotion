using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCharacterController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 7f;
    public float acceleration = 1f;
    public float crouchHeightRatio = 0.5f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private bool isGrounded;
    private bool isCrouching;
    private float originalHeight;
    private float currentSpeed;

    public event Action<float, float> OnWalk;
    public event Action<float, float> OnRun;
    public event Action OnJump;
    public event Action OnCrouch;
    public event Action OnIdle;

    public bool IsGrounded { get { return isGrounded; } }
    public bool IsCrouching { get { return isCrouching; } }
    public float CurrentSpeed { get { return currentSpeed; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider != null ? capsuleCollider.height : 1.0f;
    }

    void Update()
    {
        HandleJumpInput();
        HandleCrouchInput();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontal, 0, vertical);
        bool isMoving = inputVector.magnitude > 0.1f;

        float targetSpeed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        if (isMoving)
        {
            if (isCrouching)
            {
                OnCrouch?.Invoke();
            }
            else
            {
                Debug.Log("currentSpeed: " + currentSpeed + " walkSpeed:" + walkSpeed);
                if (currentSpeed > walkSpeed)
                    OnRun?.Invoke(horizontal, vertical);
                else
                    OnWalk?.Invoke(horizontal, vertical);
            }
        }
        else
        {
            currentSpeed = 0;
            if (isCrouching)
            {
                OnCrouch?.Invoke();
            }else
                OnIdle?.Invoke();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OnJump?.Invoke();
        }
    }

    private void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        vertical = -vertical;
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * currentSpeed;
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement) * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void Crouch()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalHeight * crouchHeightRatio;
        }
        isCrouching = true;
    }

    private void StandUp()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalHeight;
        }
        isCrouching = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
        {
            isGrounded = false;
        }
    }
}
