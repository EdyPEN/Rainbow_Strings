using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRigidBody;
    Animator animator;

    public GameObject player;

    // Info
    public Vector2 playerVelocity;

    // Inputs
    public int xInput;
    public bool jumpInputTap, jumpInputHold, jumpInputRelease;

    // Walking
    public int playerFacingDirection;
    public float walkingSpeed;

    // Jump
    public bool grounded;
    public float jumpForce, maxFallSpeed, pushDownForce;

    //Jump Buffering
    public float jumpBufferingTime;
    public float jumpBufferingTimer;
    public bool playerWantsToJump;

    // Jump Coyote Time
    public float coyoteTime;
    public float coyoteTimer;

    // Jump Fix
    public bool playerJumped;

    // Taking Damage
    public bool playerIsStunned;
    public bool playerIsInvincible;
    public float playerInvincibilityTime;
    public float playerInvincibilityTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidBody = player.GetComponent<Rigidbody2D>();

        jumpBufferingTimer = jumpBufferingTime;
        
        playerInvincibilityTimer = playerInvincibilityTime;

        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isJumping", !grounded); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        playerVelocity = playerRigidBody.linearVelocity;

        // Flip Player
        if (playerFacingDirection == -1)
        {
            player.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
        }

        // Detect Walking Input
        if (!playerIsStunned)
        {
            if (Input.GetKey(KeyCode.A))
            {
                xInput = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                xInput = 1;
            }
            else
            {
                xInput = 0;
            }

            if (xInput != 0)
            {
                playerFacingDirection = xInput;
            }
        }

        // Detect Jumping Inputs
        jumpInputTap = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        jumpInputHold = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
        jumpInputRelease = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W);

        if (jumpInputTap)
        {
            playerWantsToJump = true;
            jumpBufferingTimer = jumpBufferingTime;
        }
        if (jumpInputRelease)
        {
            playerWantsToJump = false;
            playerJumped = false;
        }
    }

    private void FixedUpdate()
    {
        //PlayerAnimation();

        Walking();

        Jumping();

        JumpBuffering();

        JumpCoyoteTime();

        ApplyPushDownForce();

        InvincibilityTime();

        FallingSpeedCap();
    }

    //void PlayerAnimation()
    //{
    //    animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocityX));
    //    animator.SetFloat("yVelocity", (rb.linearVelocityY));
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D collisionPoint = collision.GetContact(0);

        Vector2 collisionNormal = collisionPoint.normal;

        if (collision.gameObject.CompareTag("Ground") && collisionNormal.x < 0.2 && collisionNormal.y > 0.8)
        {
            if (playerIsStunned)
            {
                playerIsStunned = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D collisionPoint = collision.GetContact(0);

        Vector2 collisionNormal = collisionPoint.normal;

        if (collision.gameObject.CompareTag("Ground") && collisionNormal.x < 0.2 && collisionNormal.y > 0.8)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    void PlayerAnimation()
    {
        animator.SetFloat("xVelocity", Math.Abs(playerRigidBody.linearVelocityX));
        animator.SetFloat("yVelocity", (playerRigidBody.linearVelocityY));
        animator.SetBool("isJumping", !grounded);
    }

    void Walking()
    {
        if (!playerIsStunned)
        {
            playerRigidBody.linearVelocityX = xInput * walkingSpeed * 100 * Time.fixedDeltaTime;
        }
    }

    void Jumping()
    {
        if (playerWantsToJump && coyoteTimer > 0)
        {
            playerRigidBody.linearVelocityY = jumpForce;
            playerWantsToJump = false;
            playerJumped = true;
            coyoteTimer = 0;
        }
    }

    void JumpBuffering()
    {
        if (playerWantsToJump)
        {
            jumpBufferingTimer -= Time.fixedDeltaTime;
            if (jumpBufferingTimer < 0)
            {
                playerWantsToJump = false;
            }
        }
        else
        {
            jumpBufferingTimer = 0;
        }
    }

    void JumpCoyoteTime()
    {
        if (playerVelocity.y < 0)
        {
            playerJumped = false;
        }
        if (playerJumped)
        {
            grounded = false;
            coyoteTimer = 0;
        }

        if (grounded)
        {
            coyoteTimer = coyoteTime;
        }
        else if (coyoteTimer > 0)
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
        else
        {
            coyoteTimer = 0;
        }
    }

    void ApplyPushDownForce()
    {
        if (!grounded && (playerRigidBody.linearVelocityY < 0) && playerRigidBody.linearVelocityY > -maxFallSpeed)
        {
            playerRigidBody.AddForceY(-pushDownForce / 2.5f);
        }
        else if (!grounded && (!jumpInputHold) && playerRigidBody.linearVelocityY > -maxFallSpeed)
        {
            playerRigidBody.AddForceY(-pushDownForce);
        }
    }
    void InvincibilityTime()
    {
        if (playerIsStunned)
        {
            playerIsInvincible = true;
            playerInvincibilityTimer = playerInvincibilityTime;
        }
        if (playerIsInvincible && !playerIsStunned)
        {
            playerInvincibilityTimer -= Time.fixedDeltaTime;
            if (Mathf.CeilToInt(playerInvincibilityTimer * 10) % 2 == 1)
            {
                player.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                player.GetComponent<SpriteRenderer>().enabled = true;
            }
            if (playerInvincibilityTimer < 0)
            {
                playerIsInvincible = false;
            }
        }
        else
        {
            playerInvincibilityTimer = playerInvincibilityTime;
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    void FallingSpeedCap()
    {
        playerRigidBody.linearVelocityY = Mathf.Clamp(playerRigidBody.linearVelocityY, -maxFallSpeed, Mathf.Infinity);
    }
}
