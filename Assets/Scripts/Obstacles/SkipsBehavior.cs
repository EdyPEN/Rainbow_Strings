using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static MusicPlay;

public class SkipsBehaviour : MonoBehaviour
{
    MusicPlay playerKey;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    Animator animator;
    public bool isGrounded;

    AudioSkips audioSkips;
    private MusicKey lastPlayedKey = MusicKey.Idle;

    [Header("About Player")]
    public MusicPlay ColorDisplay;

    public GameObject[] Note;

    [Header("Variables")]
    public int damage = 1;
    public int jump = 3;
    public int direction = 1;
    public int currentNote;
    public float speed, force;
    public bool playerInRange;

    [Header("Skips' death")]
    public bool dead;
    public int combo = 0;
    public float timerIdle;
    public float timerHit;
    public float deathJumpTime;
    public float deathJumpTimer;
    public float deathJumpHeight;

    [Header("Pattern")]
    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[3];
    public MusicKey key;

    void Start()
    {
        Note[0].SetActive(false);
        Note[1].SetActive(false);
        Note[2].SetActive(false);
        lastPlayedKey = MusicKey.Idle;
        audioSkips = AudioSkips.Instance;

        ColorDisplay = GameObject.FindGameObjectWithTag("ColorDisplay").GetComponent<MusicPlay>();

        isGrounded = true;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (ColorDisplay == null)
        {
            ColorDisplay = GetComponent<MusicPlay>();
        }

        //playerKey = ColorDisplay.GetComponent<MusicPlay>();
        playerKey = ColorDisplay;

        deathJumpTimer = deathJumpTime;

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

        RemoveFromScene();
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
        if (!isGrounded)
        {
            return;
        }

        // Play SFX only when the key actually changes
        if (key != lastPlayedKey)
        {
            PlayKeySfxOnce(key);
            lastPlayedKey = key;
        }

        if (key == MusicKey.Idle)
        {
            spriteRenderer.color = Color.white;
        }
        else if (key == MusicKey.Yellow)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteYellow);
            spriteRenderer.color = Color.yellow;
        }
        else if (key == MusicKey.Green)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteGreen);
            spriteRenderer.color = Color.green;
        }
        else if (key == MusicKey.Blue)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteBlue);
            spriteRenderer.color = Color.deepSkyBlue;
        }
        else if (key == MusicKey.Red)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteRed);
            spriteRenderer.color = Color.red;
        }
    }
    void PlayKeySfxOnce(MusicKey newKey)
    {
        if (audioSkips == null)
        {
            return;
        }

        if (newKey == MusicKey.Yellow)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteYellow);
        }
        else if (newKey == MusicKey.Green)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteGreen);
        }
        else if (newKey == MusicKey.Blue)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteBlue);
        }
        else if (newKey == MusicKey.Red)
        {
            audioSkips.PlaySFX(audioSkips.SkipsNoteRed);
        }
        // Idle - no sound
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
        if (combo == 3 && !dead)
        {
            dead = true;

            rigidBody.linearVelocity = new Vector2(0, deathJumpHeight);

            rigidBody.gravityScale = 2;

            GetComponent<Collider2D>().enabled = false;
        }
    }

    void RemoveFromScene()
    {
        if (dead)
        {
            deathJumpTimer -= Time.deltaTime;
            if (deathJumpTimer < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Jump()
    {
        if (timerIdle != 0)
        {
            return;
        }

        rigidBody.linearVelocityY = 0;
        rigidBody.linearVelocityX = direction * speed;
        rigidBody.AddForceY(force);

        // Show current required note color
        key = pattern[currentNote];

        // Advance index for the next jump
        currentNote++;
        if (currentNote >= pattern.Length)
        {
            currentNote = 0;
        }

        jump++;
    }

    void ChangeDirection()
    {
        if (direction == 1)
        {
            spriteRenderer.flipX = false;
            direction = -1;
        }
        else if (direction == -1)
        {
            spriteRenderer.flipX = true;
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
        if (jump >= 3)
        {
            timerIdle = 2;
            ChangeDirection();
            //Jump();
            jump = 0;
            key = MusicKey.Idle;
            return;
        }

        Jump();
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

        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
