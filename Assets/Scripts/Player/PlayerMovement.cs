using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public GameObject player;

    // Info
    public Vector2 playerVelocity;

    // Inputs
    private int xInput;
    public bool jumpInputHold, jumpInputTap;

    // Walking
    public int playerFacingDirection;
    public float walkingSpeed;

    // Jump
    private bool grounded;
    public float jumpForce, maxFallSpeed, pushDownForce;

    //Jump Buffering
    public float jumpBufferingTime;
    public float jumpBufferingTimer;
    public bool playerWantsToJump;

    // Taking Damage
    public bool playerIsStunned;
    public bool playerIsInvincible;
    public float playerInvincibilityTime;
    public float playerInvincibilityTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();

        jumpBufferingTimer = jumpBufferingTime;

        playerInvincibilityTimer = playerInvincibilityTime;
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = rb.linearVelocity;
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

        jumpInputHold = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);

        jumpInputTap = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);


        if (jumpInputTap)
        {
            playerWantsToJump = true;
            jumpBufferingTimer = jumpBufferingTime;
        }
        if (!jumpInputHold)
        {
            playerWantsToJump = false;
        }
    }

    private void FixedUpdate()
    {
        Walking();

        Jumping();

        JumpBuffering();

        ApplyPushDownForce();

        InvincibilityTime();

        FallingSpeedCap();
    }
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
    void Walking()
    {
        if (!playerIsStunned)
        {
            rb.linearVelocityX = xInput * walkingSpeed * 100 * Time.fixedDeltaTime;
        }
    }
    void Jumping()
    {
        if (playerWantsToJump && grounded)
        {
            rb.linearVelocityY = jumpForce;
            playerWantsToJump = false;
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
    void ApplyPushDownForce()
    {
        if (!grounded && (rb.linearVelocityY < 0) && rb.linearVelocityY > -maxFallSpeed)
        {
            rb.AddForceY(-pushDownForce / 2.5f);
        }
        else if (!grounded && (!jumpInputHold) && rb.linearVelocityY > -maxFallSpeed)
        {
            rb.AddForceY(-pushDownForce);
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
        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxFallSpeed, Mathf.Infinity);
    }
}
