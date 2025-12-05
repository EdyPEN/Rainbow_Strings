using System;
using Unity.VisualScripting;
using UnityEngine;
using static MusicPlay;

public class HiddenPlatformBehaviour : MonoBehaviour
{
    public GameObject MusicRange;
    public GameObject ColorDisplay;
    public GameObject HiddenPlatform;
    public Transform Transform;
    SpriteRenderer sr;

    public int currentNote;

    public bool playerInRange;

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[3];
    public MusicKey key;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        PatternRandomizer(pattern);

        ColorDisplay.GetComponent<MusicPlay>().key = pattern[0];
        Transform.localScale = new Vector2(0.25f, 0.25f);
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
        ColorLogic();

        if (playerInRange == true)
        {
            if (ColorDisplay.GetComponent<MusicPlay>().key == pattern[currentNote])
            {
                currentNote++;
            }

            if (currentNote < 3)
            {
                key = pattern[currentNote];
                Transform.localScale = new Vector2(0.25f * (currentNote + 1), 0.25f * (currentNote + 1));
            }
            else
            {
                key = MusicKey.Idle;
                Transform.localScale = Vector2.zero;
                HiddenPlatform.GetComponent<Collider2D>().isTrigger = false;
                HiddenPlatform.GetComponent<SpriteRenderer>().enabled = true;
            }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MusicRange")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MusicRange")
        {
            playerInRange = false;
        }
    }
}
