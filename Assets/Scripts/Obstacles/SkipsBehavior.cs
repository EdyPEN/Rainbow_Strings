using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static MusicPlay;

public class SkipsBehaviour : MonoBehaviour
{
    [Header("About Player")]
    public GameObject AreaInteraction;
    public GameObject ColorDisplay;

    public GameObject[] Note;

    [Header("Variables")]
    public int damage = 1;
    public int jump = 3;
    public int direction = 1;
    public int currentPattern;
    public float speed, force;

    [Header("Skips' death")]
    public int combo = 0;
    public float timerIdle;
    public float timerHit;

    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Pattern")]
    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[3];
    public MusicKey key;

    void Start()
    {
        PatternRandomizer(pattern);

        Note[0].SetActive(false);
        Note[1].SetActive(false);
        Note[2].SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    void PatternRandomizer(MusicPlay.MusicKey[] pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            int[] keys = (int[])Enum.GetValues(typeof(MusicPlay.MusicKey));
            int minKey = Mathf.Min(keys) + 1;
            int maxKey = Mathf.Max(keys);

            int newKey = UnityEngine.Random.Range(minKey, maxKey + 1);

            if (i != 0)
            {
                if (newKey == (int)pattern[i - 1])
                {
                    ++newKey;
                    if (newKey > maxKey)
                        newKey = minKey;
                }
            }
            pattern[i] = (MusicPlay.MusicKey)newKey;
        }
    }
    void Update()
    {
        TimerLogic();
        ColorLogic();

        // Interaction without area dependens
        if (combo == 3)
        {
            Destroy(gameObject);
        }
    }
    void TimerLogic()
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
    }
    void ColorLogic()
    {
        if (key == MusicKey.Idle)
        {
            sr.color = Color.white;
        }
        else if (key == MusicKey.Yellow)
        {
            sr.color = Color.yellow;
        }
        else if (key == MusicKey.Green)
        {
            sr.color = Color.green;
        }
        else if (key == MusicKey.Blue)
        {
            sr.color = Color.deepSkyBlue;
        }
        else if (key == MusicKey.Red)
        {
            sr.color = Color.red;
        }
    }

    void Interaction()
    {
        if ((ColorDisplay.GetComponent<MusicPlay>().key == pattern[currentPattern]) && (combo >= 0))
        {
            Note[0].GetComponent<Note>().key = pattern[currentPattern];
            combo = 1;
            timerHit = 2;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().key == pattern[currentPattern]) && (timerHit > 0) && (combo >= 1))
        {
            Note[1].GetComponent<Note>().key = pattern[currentPattern];
            combo = 2;
            timerHit = 2;
        }
        else if ((ColorDisplay.GetComponent<MusicPlay>().key == pattern[currentPattern]) && (timerHit > 0) && (combo >= 2))
        {
            Note[2].GetComponent<Note>().key = pattern[currentPattern];
            timerHit = 2;
            combo = 3;
        }
        else if (ColorDisplay.GetComponent<MusicPlay>().key != MusicPlay.MusicKey.Idle)
        {
            combo = 0;
            Note[0].GetComponent<Note>().key = MusicKey.Idle;
            Note[1].GetComponent<Note>().key = MusicKey.Idle;
            Note[2].GetComponent<Note>().key = MusicKey.Idle;
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
            key = MusicKey.Idle;
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

            key = pattern[currentPattern];

            if (currentPattern == 2)
            {
                currentPattern = 0;
            }
            else
            {
                currentPattern++;
            }

            // PATTERN IS NOT PATTERING ------------------------------------------------------!!!!

            if (key == MusicKey.Idle && timerIdle == 0)
            {
                key = pattern[currentPattern];
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
        if (collision.gameObject.tag == "Ground")
        {
            CheckOnGround();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Area")
        {
            Note[0].SetActive(true);
            Note[1].SetActive(true);
            Note[2].SetActive(true);
            Interaction();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Area")
        {
            Note[0].SetActive(false);
            Note[1].SetActive(false);
            Note[2].SetActive(false);
        }
    }
}
