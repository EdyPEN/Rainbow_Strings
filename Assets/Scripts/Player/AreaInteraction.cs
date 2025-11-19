using UnityEngine;

public class AreaInteraction : MonoBehaviour
{
    SpriteRenderer sr;
    MusicPlay musicPlay;
    public GameObject ColorDisplay;

    public float circleAlpha = 0.3f; // Transparency of the circle

    private int idle = 0;
    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        musicPlay = ColorDisplay.GetComponent<MusicPlay>();

        Color c = sr.color;
        c.a = circleAlpha; // Make circle semi-transparent
        sr.color = c;
    }

    void Update()
    {
        Color c = sr.color;

        int currentColor = musicPlay.color;

        if (currentColor == idle)
        {
            c.r = 1f;
            c.g = 1f;
            c.b = 1f;
            c.a = circleAlpha;
        }
        else if (currentColor == yellow)
        {
            c.r = 1f;
            c.g = 1f;
            c.b = 0f;
            c.a = circleAlpha;
        }
        else if (currentColor == green)
        {
            c.r = 0f;
            c.g = 1f;
            c.b = 0f;
            c.a = circleAlpha;
        }
        else if (currentColor == blue)
        {
            c.r = 0f;
            c.g = 0.5f;
            c.b = 1f;
            c.a = circleAlpha;
        }
        else if (currentColor == red)
        {
            c.r = 1f;
            c.g = 0f;
            c.b = 0f;
            c.a = circleAlpha;
        }

        sr.color = c;
    }
}

