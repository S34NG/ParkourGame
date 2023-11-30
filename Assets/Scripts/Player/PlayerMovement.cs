using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Move Info
    [Header("Move Info")]
    [SerializeField] private float speed = 2f;


    // Ground Check 
    [Header("Ground Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    // Jump
    [Header("Jump Insert Info")]
    [SerializeField] private float jumpForce = 12f;

    // Double Jump
    private bool canDoubleJump;

    // Referrence
    private Rigidbody2D rb;
    private Animator anim;
    private bool canMove = true;
    private float moveInput;

    // Check a Positin Face Right or Left
    private bool facingRight = true;
    private int facingDirection = 1;

    // Wall check for Jump and Slide down
    [Header("Wall Info")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Vector2 wallJumpDirection;
    private bool isWallDetected;
    private bool canWallSliding;
    private bool isWallSliding;
    private bool isDoubleJump;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        FlipController();
        CollisionCheck();
        AnimatorController();
        CheckInput();

        //Check Ground
        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
        }


        Move();
    }


    // Animator and Animation
    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isMoving", isMoving);
        // anim.SetBool("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);

    }


    // Check a key Input Movement and Jump
    // Function
    private void CheckInput()
    {
        // Movement
        moveInput = Input.GetAxisRaw("Horizontal");


        // Jump Key "Space"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }

    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }

    // Flip 
    // For Character face flip left or right when the xVelocity is true
    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // Flip Controller 
    private void FlipController()
    {
        if (isGrounded)
        {
            if (facingRight && moveInput < 0)
            {
                Flip();
            }
            else if (!facingRight && moveInput > 0)
            {
                Flip();
            }
        }

        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }

    }

    // Jump Controller 
    private void JumpButton()
    {
        if(isGrounded)
        {
            Jump();
        }else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Double Jump


    // Wall Sliding

    
    //


    // Collision Check
    // Ground and Wall Check
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);



    }

    // Draw Gimos
    // Draw a line for detect the ground or wall
    private void OnDrawGizmos()
    {
        //Ground
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }


}
