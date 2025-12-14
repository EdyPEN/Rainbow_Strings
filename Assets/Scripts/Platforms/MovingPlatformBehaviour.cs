using System;
using UnityEngine;
using static MusicPlay;

public class MovingPlatform : MonoBehaviour
{
    [Header ("Points")]
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;

    [Header("Movement")]
    public float speed = 2;
    public bool isMoving = false;
    public bool isReturning = false;

    [Header("ResetTime")]
    public float platformResetTime = 2f;
    public float platformTimer;

    [Header("Positions")]
    public Vector2[] nextPos = new Vector2[4];
    public int currentPosition = 0;

    
    [Header("Player's Interactions")]
    MusicPlay playerKey;
    public GameObject ColorDisplay;
    public bool playerIsInRange;

    [Header("Pattern & Colors")]
    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];
    public MusicKey key;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        PatternRandomizer(pattern);

        key = pattern[0];

        nextPos[0] = pointA.position;
        nextPos[1] = pointB.position;
        nextPos[2] = pointC.position;
        nextPos[3] = pointD.position;

        key = pattern[0];

        platformTimer = platformResetTime;

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerKey = ColorDisplay.GetComponent<MusicPlay>();
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

    // Update is called once per frame
    void Update()
    {
        ColorLogic();

        if (isMoving)
        {
            MovePlatform();
            return;
        }

        // 1) Try progress forward if player is in range
        if (playerIsInRange && !isReturning)
        {
            InteractionForward();
        }

        // 2) Handle rollback when stuck mid-way or after completion
        HandleResetTime();

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
    void MovePlatform()
    {
        transform.position = Vector2.MoveTowards
            (transform.position, nextPos[currentPosition], speed * Time.deltaTime);

        if (transform.position.x == nextPos[currentPosition].x 
            && 
            transform.position.y == nextPos[currentPosition].y)
        {
            isMoving = false;
        }

        float dx = Mathf.Abs(transform.position.x - nextPos[currentPosition].x);
        float dy = Mathf.Abs(transform.position.y - nextPos[currentPosition].y);

        if (dx < 0.001f && dy < 0.001f)
        {
            transform.position = nextPos[currentPosition];
            isMoving = false;

            // When NOT returning -> update expected key and reset timer
            if (!isReturning)
            {
                UpdateExpectedKeyByPosition();

                if (currentPosition > 0)
                {
                    platformTimer = platformResetTime;
                }
                return;
            }
            // When returning: go back step-by-step
            if (currentPosition > 0)
            {
                platformTimer = platformResetTime;

                int nextBack = currentPosition - 1;
                if (nextBack < 0)
                {
                    nextBack = 0;
                }
                GoToPosition(nextBack);
            }
            else
            {
                isReturning = false;
                platformTimer = platformResetTime;
                UpdateExpectedKeyByPosition();
            }
            return;
        }
    }
    void GoToPosition(int newPos)
    {
        currentPosition = newPos;
        isMoving = true;
    }
    void UpdateExpectedKeyByPosition()
    {
        // At A expect pattern[0], at B expect pattern[1], at C expect pattern[2], at D -> Idle
        if (currentPosition == 0)
        {
            key = pattern[0];
        }
        else if (currentPosition == 1)
        {
            key = pattern[1];
        }
        else if (currentPosition == 2)
        {
            key = pattern[2];
        }
        else
        {
            key = MusicKey.Idle;
        }
    }
    void HandleResetTime()
    {
        // If we are at start, nothing to go back
        if (currentPosition == 0)
        {
            isReturning = false;
            platformTimer = platformResetTime;
            return;
        }

        // Count down
        platformTimer -= Time.deltaTime;
        if (platformTimer > 0f)
        {
            return;
        }

        // Start returning (only once)
        if (!isReturning)
        {
            isReturning = true;
            key = MusicKey.Idle;

            int nextBack = currentPosition - 1;
            if (nextBack < 0)
            {
                nextBack = 0;
            }

            GoToPosition(nextBack);
        }
    }
    void InteractionForward()
    {
        if (playerKey.key == MusicKey.Idle)
        {
            return;
        }

        // Position A -> needs pattern[0] -> to B
        if (currentPosition == 0)
        {
            if (playerKey.key == pattern[0])
            {
                GoToPosition(1);
                return;
            }
        }
        // Position B -> pattern[1] -> to C
        else if (currentPosition == 1)
        {
            if (playerKey.key == pattern[1])
            {
                GoToPosition(2);
                return;
            }
        }
        // Position C -> needs pattern[2] -> to D
        else if (currentPosition == 2)
        {
            if (playerKey.key == pattern[2])
            {
                GoToPosition(3);
                return;
            }
        }

        // Wrong key = no progress. We don't instantly reset, we let rollback timer handle it.
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
            playerIsInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("MusicRange"))
        {
            playerIsInRange = false;
        }
    }
}
