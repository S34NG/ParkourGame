using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Move Info
    [Header("Move Info")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private float speed = 2f;

    // Ground Check
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool canDoubleJump;

    // Wall Check and Slide
    [Header("Wall Info")]
    [SerializeField] private float wallCheckDistance;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;

    // Wall Jump Check
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    //Buffer Jump
    [Header("Buffer Jump Info")]
    [SerializeField] private float bufferJumpTime;
    private float bufferJumpCounter;

    // Cayote Jump
    [Header("Cayote Jump Info")]
    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpCounter;
    private bool canHaveCayoteJump;

    // Knock Back
    [Header("Knockback info")]
    [SerializeField] private Vector2 knockBackDirection;
    [SerializeField] private float knockBackTime;
    [SerializeField] private float knockBackProtectionTime;
    private bool isKnock;
    private bool canBeKnock = true;

    // Reference
    private bool canMove = true;

    private Animator animator;
    private Rigidbody2D rb;

    private bool facingRight = true;
    private float moveInput;

    private float defaultJumpForce;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        defaultJumpForce = jumpForce;

    }

    void Update()
    {
        FlipController();
        CollisionCheck();
        AnimatorController();

        if(isKnock)
        {
            return;
        }

        CheckInput();
        
        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        // Check Ground
        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;

            if(bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }
            canHaveCayoteJump = true;

        }
        else
        {
            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;

            }
        }

        // Check wall Detected and Slide
        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.3f);
        }
        Move();
    }

    // Check input == Check a key input for movement and jump
    private void CheckInput()
    {

        moveInput = Input.GetAxisRaw("Horizontal");

        // Pushing the player go down fast when press the vertical key 
        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }

        // Jump key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }


    }

    // Move = player Movement
    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        animator.SetFloat("yVelocity", rb.velocity.y);

        animator.SetBool("isGrounded", isGrounded);

        animator.SetBool("isMoving", isMoving);

        animator.SetBool("isWallSliding", isWallSliding);

        animator.SetBool("isWallDetected", isWallDetected);

        animator.SetBool("isKnock", isKnock);
        

    }

    // Flip 
    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    // Flip Controller
    private void FlipController()
    {

        if (isGrounded && isWallDetected)
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

    //Jump

    private void JumpButton()
    {

        if(!isGrounded)
        {
            bufferJumpCounter = bufferJumpTime;
        }

        if (isGrounded || cayoteJumpCounter > 0)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;

        }
        else if (isWallSliding)
        {
            WallJump();
        }

        canWallSlide = false;

    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Wall Jump Function
    private void WallJump()
    {
        canMove = false;
        canDoubleJump = true;

        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

    }

    // Kncokback
    public void KnockBack(Transform damageTransform)
    {
        if (!canBeKnock)
        {
            return;
        }

        isKnock = true;
        canBeKnock = false;

        #region Define horizontal direction for knockback
        int hDirection = 0;

        if (transform.position.x > damageTransform.position.x)
        {
            hDirection = 1;
        }
        else if (transform.position.x < transform.position.x)
        {
            hDirection = -1;
        }
        else
        {
            hDirection = 0;
        }

        #endregion

        rb.velocity = new Vector2(knockBackDirection.x * hDirection, knockBackDirection.y);

        Invoke("CancelKnockBack", knockBackTime);
        Invoke("AllowKnockBack", knockBackProtectionTime);
    }

    private void CancelKnockBack()
    {
        isKnock = false;
    }

    private void AllowKnockBack()
    {
        canBeKnock = true;
    }


    // Ground Check 
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (isWallDetected && rb.velocity.y < 0)
        {
            canWallSlide = true;
            canDoubleJump = false;
        }

        if (!isWallDetected)
        {
            canWallSlide = false;
            isWallSliding = false;
        }


    }

    // Draw Gimos
    private void OnDrawGizmos()
    {
        // Ground
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        //Wall
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
    }

}
