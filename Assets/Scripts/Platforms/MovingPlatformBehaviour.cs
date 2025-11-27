using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header ("Points")]
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;

    [Header("Variables")]
    public float speed = 2;
    public bool isMoving = false;

    public Vector2[] nextPos = new Vector2[3];
    public int currentPosition = 0;

    // Player interaction with a platform
    public GameObject ColorDisplay;

    // Pattern & Colors
    public int[] pattern = new int[3];
    //public int currentPattern = 0;
    public int color = 1;

    //public float timerIdle;
    //public float timerHit;

    private int idle = 0;
    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    SpriteRenderer sr;

    void Start()
    {
        pattern[0] = blue;
        pattern[1] = red;
        pattern[2] = yellow;

        nextPos[0] = pointA.position;
        nextPos[1] = pointB.position;
        nextPos[2] = pointC.position;
        nextPos[3] = pointD.position;

        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }

        //if (timerHit > 0)
        //{
        //    timerHit -= Time.deltaTime;
        //}
        //else
        //{
        //    timerHit = 0;
        //}

        // Color Stuff
        if (color == idle)
        {
            sr.color = Color.white;
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

    void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextPos[currentPosition], speed * Time.fixedDeltaTime);

        if (transform.position.x == nextPos[currentPosition].x && transform.position.y == nextPos[currentPosition].y)
        {
            isMoving = false;
        }
    }

    void Interaction()
    {
        if ((ColorDisplay.GetComponent<MusicPlay>().color == blue) && (currentPosition == 0) && (!isMoving))
        {
            isMoving = true; 
            currentPosition = 1;
            color = red;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().color == red)/* && (timerHit > 0)*/ && (currentPosition == 1) && (!isMoving))
        {
            isMoving = true;
            currentPosition = 2;
            color = yellow;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().color == yellow)/* && (timerHit > 0)*/ && (currentPosition == 2) && (!isMoving))
        {
            isMoving = true;
            currentPosition = 3;
            color = idle;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("MusicRange"))
        {
            Interaction();
        }
    }
}
