using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static MusicPlay;

public class SkipsBehaviour : MonoBehaviour
{
    MusicPlay playerKey;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    [Header("About Player")]
    public GameObject ColorDisplay;
    public GameObject AreaInteraction;

    public GameObject[] Note;

    [Header("Variables")]
    public int damage = 1;
    public int jump = 3;
    public int direction = 1;
    public int currentNote;
    public float speed, force;
    public bool playerInRange;

    [Header("Skips' death")]
    public int combo = 0;
    public float timerIdle;
    public float timerHit;

    [Header("Pattern")]
    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[3];
    public MusicKey key;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerKey = ColorDisplay.GetComponent<MusicPlay>();

        PatternRandomizer(pattern);
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

        Interaction();

        Death();
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
        if (combo > 0)
        {
            if (timerHit > 0)
            {
                timerHit -= Time.deltaTime;
            }
            else
            {
                timerHit = 0;
            }
        }
    }

    void ColorLogic()
    {
        if (key == MusicKey.Idle)
        {
            spriteRenderer.color = Color.white;
        }
        else if (key == MusicKey.Yellow)
        {
            spriteRenderer.color = Color.yellow;
        }
        else if (key == MusicKey.Green)
        {
            spriteRenderer.color = Color.green;
        }
        else if (key == MusicKey.Blue)
        {
            spriteRenderer.color = Color.deepSkyBlue;
        }
        else if (key == MusicKey.Red)
        {
            spriteRenderer.color = Color.red;
        }
    }

    void Interaction()
    {
        if (!playerInRange)
        {
            return;
        }
        if (playerKey.key == pattern[combo])
        {
            Note[combo].GetComponent<Note>().key = pattern[combo];
            combo++;
            timerHit = 2;
        }
        else if (playerKey.key != MusicPlay.MusicKey.Idle || timerHit <= 0)
        {
            combo = 0;
            timerHit = 0;
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].GetComponent<Note>().key = MusicKey.Idle;
            }
        }
    }

    void Death()
    {
        if (combo == 3)
        {
            gameObject.SetActive(false);
        }
    }

    void Jump()
    {
        if (timerIdle == 0)
        {
            rigidBody.linearVelocityY = 0;
            rigidBody.linearVelocityX = direction * speed;
            rigidBody.AddForceY(force);
            jump++;

            key = pattern[currentNote];

            if (currentNote == 2)
            {
                currentNote = 0;
            }
            else
            {
                currentNote++;
            }

            // PATTERN IS NOT PATTERING ------------------------------------------------------!!!!

            if (key == MusicKey.Idle && timerIdle == 0)
            {
                key = pattern[currentNote];
            }
        }
        else if (jump == 3)
        {
            rigidBody.AddForceY(-1 * force);
            rigidBody.linearVelocity = Vector2.zero;
        }
    }

    void ChangeDirection()
    {
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            CheckOnGround();
        }
    }

    void CheckOnGround()
    {
        if (jump == 3)
        {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage, true, collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Area")
        {
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].SetActive(true);
            }
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Area")
        {
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].SetActive(false);
            }
            playerInRange = false;
        }
    }
}
