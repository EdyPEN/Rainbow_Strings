using System;
using Unity.VisualScripting;
using UnityEngine;
using static MusicPlay;

public class HiddenPlatformBehaviour : MonoBehaviour
{
    SpriteRenderer sr;
    MusicPlay playerKey;
    Collider2D collider2D;

    public GameObject ColorDisplay;

    public int currentNote;

    public bool playerInRange;

    public Vector2 platformScale;

    public float platformResetTime;
    public float platformResetTimer;

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];
    public MusicKey key;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
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
        else
        {
            sr.color = new Color32(100, 50, 0, 255);
        }
    }

    void UpdateScale()
    {
        transform.localScale = platformScale * (0.25f * (currentNote + 1));
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
