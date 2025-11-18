using UnityEngine;

public class SkipsBehaviour : MonoBehaviour
{
    [Header("About Player")]
    public GameObject AreaInteraction;
    public GameObject ColorDisplay;

    [Header("Variables")]
    public int damage = 1;
    public int jump = 3;
    public int direction = 1;
    public int color = 0;
    public float speed, force;

    private int idle = 0;
    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    [Header("Skips' death")]
    public int combo = 0;
    public float timerIdle;
    public float timerHit;

    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Pattern")]
    public int[] pattern = new int[3];
    public int currentPattern = 0;

    void Start()
    {
        // pattern 3, 1, 2
        pattern[0] = green;
        pattern[1] = red;
        pattern[2] = yellow;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // TIMER IDLE ---------------------
        if (timerIdle > 0)
        {
            timerIdle -= Time.deltaTime;
        }
        else
        {
            timerIdle = 0;
        }
        if (timerIdle == 0 && jump == 0)
        {
            CheckOnGround();
        }

        // TIMER HIT ---------------------
        if (timerHit > 0)
        {
            timerHit -= Time.deltaTime;
        }
        else
        {
            timerHit = 0;
        }

        // COLOR CHANGE ---------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (color == idle)
        {
            sr.color = Color.black;
        }
        else if (color == yellow)
        {
            sr.color = Color.yellow;
        }
        else if (color == green)
        {
            sr.color = Color.green;
        }
        else if (color == blue)
        {
            sr.color = Color.blue;
        }
        else if (color == red)
        {
            sr.color = Color.red;
        }

    }

    void Interaction() //
    {
        if ((ColorDisplay.GetComponent<MusicPlay>().color == green) && (combo >= 0))
        {
            combo = 1;
            timerHit = 2;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().color == red) && (timerHit > 0) && (combo >= 1))
        {
            combo = 2;
            timerHit = 2;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().color == yellow) && (timerHit > 0) && (combo >= 2))
        {
            Destroy(gameObject);
        }
        else
        {
            combo = 0;
        }
    }

    void CheckOnGround()
    {
        //print("GROUND check");
        if (jump == 3)
        {
            //print("GROUND call !!!!!");
            timerIdle = 2;
            ChangeDirection();
            Jump();
            jump = 0;
            color = 0;
        }
        else if (jump < 3)
        {
            Jump();
        }
    }

    void Jump()
    {
        //print("Jump: " + jump);
        if (timerIdle == 0)
        {
            rb.linearVelocityY = 0;
            rb.linearVelocityX = direction * speed;
            rb.AddForceY(force);
            jump++;

            color = pattern[currentPattern];

            if (currentPattern == 2)
            {
                currentPattern = 0;
            }
            else
            {
                currentPattern++;
            }

            // PATTERN IS NOT PATTERING ------------------------------------------------------!!!!

            if (color == 0 && timerIdle == 0)
            {
                color = 1;
            }
        }
        else if (jump == 3)
        {
            rb.AddForceY(-1 * force);
            rb.linearVelocityY = 0;
            rb.linearVelocityX = 0;
        }
    }

    void ChangeDirection()
    {
        //print("DIRECTION called");
        if (direction == 1)
        {
            direction = -1;
        }
        else if (direction == -1)
        {
            direction = 1;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //collision.gameObject.GetComponent<Movement>().hp -= damage; !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
        if (collision.gameObject.tag == "Ground")
        {
            CheckOnGround();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Area")
        {
            Interaction();
        }
    }
}
