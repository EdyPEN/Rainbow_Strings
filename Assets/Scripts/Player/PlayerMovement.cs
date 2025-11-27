using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    // Inputs
    private int xInput;

    public bool jumpInputHold, jumpInputTap;

    public bool ignorePlayerMovement;

    public float invincibilityTime;
    public float invincibilityTimer;
    // Variables
    private bool grounded;

    public float speed, force, maxFallSpeed, pushDownForce;

    public int playerFacingDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFacingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ignorePlayerMovement == false)
        {
            invincibilityTimer = invincibilityTime;
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
        else
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer < 0)
            {
                ignorePlayerMovement = false;
            }
        }


            jumpInputHold = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);

        jumpInputTap = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);


        if (jumpInputTap == true && grounded)
        {
            rb.linearVelocityY = force;
        }
        else if (!grounded && (jumpInputHold == false || rb.linearVelocityY < 5) && rb.linearVelocityY > -maxFallSpeed)
        {
            rb.AddForceY(-pushDownForce);
        }
    }

    private void FixedUpdate()
    {
        if (ignorePlayerMovement == false)
        {
            rb.linearVelocityX = xInput * speed * Time.fixedDeltaTime;
        }

        rb.linearVelocityY = Mathf.Clamp(rb.linearVelocityY, -maxFallSpeed, Mathf.Infinity);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D collisionPoint = collision.GetContact(0);

        Vector2 collisionNormal = collisionPoint.normal;

        if (collision.gameObject.layer == 6 && collisionNormal.x < 0.2 && collisionNormal.y > 0.8)  // ground layer
        {
            grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) // ground layer
        {
            grounded = false;
        }
    }
}
