using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody2D playerRigidBody;
    SpriteRenderer playerSpriteRenderer;

    public GameObject player;

    // Info
    public Vector2 playerVelocity;

    [Header("Inputs")]
    // Inputs
    public int xInput;
    public bool jumpInputTap;
    public bool jumpInputHold;
    public bool jumpInputRelease;

    [Header("Walking")]
    // Walking
    public float walkingSpeed;
    public int playerFacingDirection;

    [Header("Jump")]
    // Jump
    public bool grounded;
    bool wasGrounded; //---------------------------ehg....

    public float jumpForce;
    public float maxFallSpeed;
    public float pushDownForce;

    [Header("Jump Buffering")]
    //Jump Buffering
    public bool playerWantsToJump;
    public float jumpBufferingTime;
    public float jumpBufferingTimer;

    [Header("Jump Coyote Time")]
    // Jump Coyote Time
    public float coyoteTime;
    public float coyoteTimer;

    [Header("Jump Fix")]
    // Jump Fix
    public bool playerJumped;

    [Header("Taking Damage")]
    // Taking Damage
    public bool playerIsStunned;
    public bool playerIsInvincible;
    public float playerInvincibilityTime;
    public float playerInvincibilityTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = player.GetComponent<Animator>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FlipPlayer();

        DetectWalkingInputsIfNotStunned();

        DetectJumpingInputs();
    }

    void FlipPlayer()
    {
        if (xInput == 0)
            return;
        playerFacingDirection = xInput;

        if (playerFacingDirection == -1)
            playerSpriteRenderer.flipX = true;
        else
            playerSpriteRenderer.flipX = false;
    }

    void DetectWalkingInputsIfNotStunned()
    {
        if (playerIsStunned)
        {
            return;
        }
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
    }

    void DetectJumpingInputs()
    {
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
        PlayerAnimation();

        Walking();

        Jumping();

        JumpBuffering();

        JumpCoyoteTime();

        ApplyPushDownForce();

        InvincibilityTime();

        FallingSpeedCap();

        wasGrounded = grounded;
    }

    void PlayerAnimation()
    {
        animator.SetFloat("xVelocity", Math.Abs(playerRigidBody.linearVelocityX));
        animator.SetFloat("yVelocity", (playerRigidBody.linearVelocityY));
        animator.SetBool("isJumping", !grounded);

        if (grounded && !wasGrounded)
        {
            // We have just landed this frame
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // If we are still in Jumping state, force switch to Movement
            if (stateInfo.IsName("Jumping"))
            {
                // Play Movement from the beginning
                animator.Play("Movement", 0, 0f);
            }
        }
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
                playerSpriteRenderer.enabled = false;
            }
            else
            {
                playerSpriteRenderer.enabled = true;
            }
            if (playerInvincibilityTimer < 0)
            {
                playerIsInvincible = false;
            }
        }
        else
        {
            playerInvincibilityTimer = playerInvincibilityTime;
            playerSpriteRenderer.enabled = true;
        }
    }

    void FallingSpeedCap()
    {
        playerRigidBody.linearVelocityY = Mathf.Clamp(playerRigidBody.linearVelocityY, -maxFallSpeed, Mathf.Infinity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RecoverControlOnGroundContact(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        SetGroundedStateOn(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetGroundedStateOff(collision);
    }

    void RecoverControlOnGroundContact(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && GetCollisionNormal(collision).x < 0.2 && GetCollisionNormal(collision).y > 0.8)
        {
            if (playerIsStunned)
            {
                playerIsStunned = false;
            }
        }
    }

    void SetGroundedStateOn(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    void SetGroundedStateOff(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    public static Vector2 GetCollisionNormal(Collision2D collision)
    {
        ContactPoint2D collisionPoint = collision.GetContact(0);
        return collisionPoint.normal;
    }
}
