using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public void Awake() { Instance = this; }

    public bool canMove;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    public float walkSpeed;
    public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;


    public float horizontalInput;
    public float verticalInput;

    Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Transform orientation;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Footstep footstep;

    private void Start()
    {
        readyToJump = true;
    }

    private void Update()
    {
        if (orientation == null) return;

        // ground check
        grounded = Physics.Raycast(orientation.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        if(animator != null) animator.SetBool("IsGrounded", grounded);

        if (Input.GetKey(KeyCode.LeftShift)) moveSpeed = sprintSpeed; else moveSpeed = walkSpeed;

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if(canMove && orientation != null) MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        //Move animations
        if (animator != null) animator.SetFloat("verticalInput", verticalInput);
        if (animator != null) animator.SetFloat("horizontalInput", horizontalInput);

        if (UiManager.Instance.somePanelTurnedOn) return;

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();
            animator.SetBool("isJumping", true);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        if (animator != null) animator.SetBool("isJumping", false);
        readyToJump = true;
    }
}
