using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static MusicPlay;

public class HiddenPlatformBehaviour : MonoBehaviour
{
    MusicPlay playerKey;
    Collider2D platformCollider;
    SpriteRenderer spriteRenderer;

    Animator animator;

    private int smallPlatform;
    private int midPlatform;
    private int fullPlatform;

    private int lastPlatformAnim = -1;


    public GameObject Player;
    public GameObject ColorDisplay;

    public GameObject[] Note;

    public Vector2[] noteStartingPosition;

    public int currentNote;

    public bool playerInRange;

    public Vector2 platformScale;
    public Vector2 initialPlatformScale;

    public float platformResetTime;
    public float platformResetTimer;
    public float platformResetAfterCompletionTime;
    public float platformResetAfterCompletionTimer;

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];
    public MusicKey key;

    void Start()
    {
        animator = GetComponent<Animator>();

        smallPlatform = Animator.StringToHash("HidPlatSmall");
        midPlatform = Animator.StringToHash("HidPlatMid");
        fullPlatform = Animator.StringToHash("HidPlatFull");

        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerKey = ColorDisplay.GetComponent<MusicPlay>();

        PatternRandomizer(pattern);
        
        initialPlatformScale = transform.localScale;

        key = pattern[0];

        GetNoteStartingPositions();
    }
    void UpdatePlatformAnimation()
    {
        if (animator == null) return;

        int targetAnim = smallPlatform;

        // 0 -> Small
        // 1 -> Mid
        // 2 -> Full
        if (currentNote == 0)
        {
            targetAnim = smallPlatform;
        }
        else if (currentNote == 1)
        {
            targetAnim = midPlatform;
        }
        else if (currentNote >= 2 && currentNote < 3)
        {
            targetAnim = fullPlatform;
        }
        else
        {
            // when completed (>=3) keep full or whatever you want; I'll keep full
            targetAnim = fullPlatform;
        }

        if (targetAnim != lastPlatformAnim)
        {
            animator.Play(targetAnim, 0, 0f);
            lastPlatformAnim = targetAnim;
        }
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

    void GetNoteStartingPositions()
    {
        for (int i = 0; i < Note.Length; i++)
        {
            noteStartingPosition[i] = Note[i].transform.localPosition;
        }
    }

    void Update()
    {
        UpdatePlatformAnimation();

        ChangeNoteBasedOnPlayerInput();

        ChangePlatformState();

        ColorLogic();

        UpdateScale();

        TimerReset();

        UpdateNoteScales();

        UpdateNotePositions();

        UpdateNoteColors();
    }

    void ChangeNoteBasedOnPlayerInput()
    {
        if (!playerInRange)
        {
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].SetActive(false);
            }
            return;
        }
        if (currentNote == 3)
        {
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].GetComponent<Note>().hideNotes = true;
                Note[i].GetComponent<Note>().key = MusicKey.Idle;
            }
            return;
        }
        for (int i = 0; i < Note.Length; i++)
        {
            Note[i].SetActive(true);
            Note[i].GetComponent<Note>().hideNotes = false;
        }
        if (playerKey.key == MusicKey.Idle)
        {
            return;
        }
        if (playerKey.key == pattern[currentNote])
        {
            Note[currentNote].GetComponent<Note>().key = pattern[currentNote];
            platformResetTimer = platformResetTime;
            currentNote++;
        }
        else
        {
            currentNote = 0;
        }
    }

    void ChangePlatformState()
    {
        if (currentNote < 3)
        {
            key = pattern[currentNote];
            platformCollider.isTrigger = true;
        }
        else
        {
            key = MusicKey.Idle;
            platformCollider.isTrigger = false;
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
            spriteRenderer.color = Color.white;
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
            for (int i = 0; i < Note.Length; i++)
            {
                Note[i].GetComponent<Note>().key = MusicKey.Idle;
            }
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
        else
        {
            spriteRenderer.enabled = true;
        }
    }

    void UpdateNoteScales()
    {
        for (int i = 0; i < Note.Length; i++)
        {
            Note[i].transform.localScale = new Vector2(1, 1) / transform.localScale;
        }
    }

    void UpdateNotePositions()
    {
        for (int i = 0; i < Note.Length; i++)
        {
            if (Player.transform.position.y > transform.position.y)
            {
                Note[i].transform.localPosition = new Vector2(noteStartingPosition[i].x, -noteStartingPosition[i].y) / (transform.localScale / initialPlatformScale);
            }
            else
            {
                Note[i].transform.localPosition = noteStartingPosition[i] / (transform.localScale / initialPlatformScale);
            }
        }
    }

    void UpdateNoteColors()
    {
        for (int i = 0; i < Note.Length; i++)
        {
            Note[i].transform.localScale = new Vector2(1, 1) / transform.localScale;
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
