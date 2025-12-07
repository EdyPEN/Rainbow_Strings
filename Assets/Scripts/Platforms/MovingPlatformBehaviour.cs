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

    [Header("Variables")]
    public float speed = 2;
    public bool isMoving = false;

    public Vector2[] nextPos = new Vector2[3];
    public int currentPosition = 0;

    // Player interaction with a platform
    MusicPlay playerKey;

    public GameObject ColorDisplay;

    public bool playerIsInRange;

    // Pattern & Colors
    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];
    public MusicKey key;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        PatternRandomizer(pattern);

        key = pattern[0];

        nextPos[0] = pointA.position;
        nextPos[1] = pointB.position;
        nextPos[2] = pointC.position;
        nextPos[3] = pointD.position;

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
        }
        if (playerIsInRange)
        {
            Interaction();
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
    void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(transform.position, nextPos[currentPosition], speed * Time.deltaTime);

        if (transform.position.x == nextPos[currentPosition].x && transform.position.y == nextPos[currentPosition].y)
        {
            isMoving = false;
        }
    }
    void Interaction()
    {
        if ((playerKey.key == pattern[0]) && (currentPosition == 0) && (!isMoving))
        {
            isMoving = true;
            currentPosition = 1;
            key = pattern[1];
        }
        else if ((playerKey.key == pattern[1])/* && (timerHit > 0)*/ && (currentPosition == 1) && (!isMoving))
        {
            isMoving = true;
            currentPosition = 2;
            key = pattern[2];
        }
        else if ((playerKey.key == pattern[2])/* && (timerHit > 0)*/ && (currentPosition == 2) && (!isMoving))
        {
            isMoving = true;
            currentPosition = 3;
            key = MusicKey.Idle;
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
