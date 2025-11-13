using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    // Inputs
    private float xInputRaw;

    public bool jumpInputHold, jumpInputTap;

    private bool resetButton;

    // Variables
    private bool grounded;

    public float speed, force, maxFallSpeed, pushDownForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xInputRaw = Input.GetAxisRaw("Horizontal");

        jumpInputHold = Input.GetButton("Jump");

        jumpInputTap = Input.GetButtonDown("Jump");

        resetButton = Input.GetKeyDown(KeyCode.R);


        if (jumpInputTap == true && grounded)
        {
            rb.linearVelocityY = force;
        }
        else if (!grounded && (jumpInputHold == false || rb.linearVelocityY < 0) && rb.linearVelocityY > -maxFallSpeed)
        {
            rb.AddForceY(-pushDownForce);
        }


        if (resetButton == true)
        {
            rb.position = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocityX = xInputRaw * speed * Time.fixedDeltaTime;

        if (rb.linearVelocityY < -maxFallSpeed)
        {
            rb.linearVelocityY = -maxFallSpeed * Time.fixedDeltaTime;
        }

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
