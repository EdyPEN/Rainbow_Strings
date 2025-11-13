using UnityEngine;

public class MusicPlay : MonoBehaviour
{
    public GameObject AreaInteraction;

    public float timer;

    SpriteRenderer sr;

    public int color = 0;

    private int idle = 0;
    private int red = 1;
    private int yellow = 2;
    private int green = 3;
    private int blue = 4;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }

        ColorChange();

        if (timer >= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                color = 1;
                timer = 1;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                color = 2;
                timer = 1;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                color = 3;
                timer = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                color = 4;
                timer = 1;
            }
            else if (timer == 0)
            {
                color = 0;
            }
        }
    }

    void ColorChange()
    {
        if (color == idle)
        {
            sr.color = Color.white;
        }
        else if (color == yellow)
        {
            sr.color = Color.yellow;
        }
        else if (color == green)
        {
            sr.color = Color.green;
        }
        else if (color == blue)
        {
            sr.color = Color.blue;
        }
        else if (color == red)
        {
            sr.color = Color.red;
        }
    }
}
