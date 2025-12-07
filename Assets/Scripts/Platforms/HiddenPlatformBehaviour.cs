using System;
using Unity.VisualScripting;
using UnityEngine;
using static MusicPlay;

public class HiddenPlatformBehaviour : MonoBehaviour
{
    MusicPlay playerKey;
    Collider2D collider2D;
    SpriteRenderer spriteRenderer;

    public GameObject ColorDisplay;

    public GameObject[] note;

    public Vector2[] noteStartingPosition;

    public int currentNote;

    public bool playerInRange;

    public Vector2 platformScale;

    public float platformResetTime;
    public float platformResetTimer;
    public float platformResetAfterCompletionTime;
    public float platformResetAfterCompletionTimer;

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];
    public MusicKey key;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerKey = ColorDisplay.GetComponent<MusicPlay>();


        PatternRandomizer(pattern);

        key = pattern[0];
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
        ChangeNoteBasedOnPlayerInput();

        ChangePlatformState();

        ColorLogic();

        UpdateScale();

        TimerReset();

        for (int i = 0; i < note.Length; i++)
        {
            note[i].transform.localScale = new Vector2(1, 1) / transform.localScale;
        }
    }

    void ChangeNoteBasedOnPlayerInput()
    {
        if (currentNote == 3)
        {
            return;
        }
        if (playerInRange == true)
        {
            if (playerKey.key == MusicKey.Idle)
            {
                return;
            }
            if (playerKey.key == pattern[currentNote])
            {
                platformResetTimer = platformResetTime;
                currentNote++;
            }
            else
            {
                currentNote = 0;
            }
        }
    }

    void ChangePlatformState()
    {
        if (currentNote < 3)
        {
            key = pattern[currentNote];
            collider2D.isTrigger = true;
        }
        else
        {
            key = MusicKey.Idle;
            collider2D.isTrigger = false;
        }
    }

    void ColorLogic()
    {
        if (currentNote < 3)
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
        else
        {
            spriteRenderer.color = new Color32(100, 50, 0, 255);
        }
    }

    void UpdateScale()
    {
        transform.localScale = platformScale * (0.25f * (currentNote + 1));
    }

    void TimerReset()
    {
        if (currentNote == 0)
        {
            platformResetTimer = platformResetTime;
        }
        else
        {
            if (currentNote < 3)
            {
                platformResetAfterCompletionTimer = platformResetAfterCompletionTime;
                if (platformResetTimer > 0)
                {
                    platformResetTimer -= Time.deltaTime;
                    BlinkingAnimation(platformResetTime, platformResetTimer);
                }
                else
                {
                    currentNote = 0;
                    platformResetTimer = 0;
                }
            }
            else
            {
                if (platformResetAfterCompletionTimer > 0)
                {
                    platformResetAfterCompletionTimer -= Time.deltaTime;
                    BlinkingAnimation(platformResetAfterCompletionTime, platformResetAfterCompletionTimer);
                }
                else
                {
                    currentNote = 0;
                    platformResetAfterCompletionTimer = 0;
                }
            }

        }
    }

    void BlinkingAnimation(float time, float timer)
    {
        if (timer < (time / 4))
        {
            if (Mathf.CeilToInt(timer * 10) % 2 == 1)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
        else if (timer < (time / 2))
        {
            if (Mathf.CeilToInt(timer * 10) % 4 == 1)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MusicRange"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MusicRange"))
        {
            playerInRange = false;
        }
    }
}
