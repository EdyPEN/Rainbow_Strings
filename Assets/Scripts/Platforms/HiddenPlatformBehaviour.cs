using Unity.VisualScripting;
using UnityEngine;

public class HiddenPlatformBehaviour : MonoBehaviour
{
    public GameObject MusicRange;
    public GameObject PlayerColor;
    public GameObject HiddenPlatform;
    public Transform Transform;
    SpriteRenderer sr;

    public int color = 0;

    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    public int currentNote = 0;
    public int playerNote;

    public bool playerInRange;

    public int[] pattern = new int[3];

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < pattern.Length; i++)
        {
            pattern[i] = Random.Range(1, 5);
            if (i != 0)
            {
                if (pattern[i] == pattern[i - 1])
                {
                    pattern[i] = pattern[i - 1] + 1;
                    if (pattern[i] > pattern.Length - 1)
                    {
                        pattern[i] = 1;
                    }
                }
            }
        }

        color = pattern[0];
        Transform.localScale = new Vector2(0.25f, 0.25f);
    }

    void Update()
    {
        // COLOR CHANGE
        playerNote = PlayerColor.GetComponent<MusicPlay>().color;

        if (playerInRange == true)
        {
            if (playerNote == pattern[currentNote])
            {
                currentNote++;
            }

            if (currentNote < 3)
            {
                color = pattern[currentNote];
                Transform.localScale = new Vector2(0.25f * (currentNote + 1), 0.25f * (currentNote + 1));
            }
            else
            {
                color = 0;
                Transform.localScale = Vector2.zero;
                HiddenPlatform.GetComponent<Collider2D>().isTrigger = false;
                HiddenPlatform.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        if (color == yellow)
        {
            sr.color = Color.yellow;
        }
        else if (color == green)
        {
            sr.color = Color.green;
        }
        else if (color == blue)
        {
            sr.color = Color.deepSkyBlue;
        }
        else if (color == red)
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
