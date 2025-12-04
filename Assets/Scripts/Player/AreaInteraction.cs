using UnityEngine;
using static MusicPlay;

public class AreaInteraction : MonoBehaviour
{
    SpriteRenderer sr;
    public MusicPlay.MusicKey key;
    public MusicKey currentKey;
    public GameObject ColorDisplay;
    
    public float circleAlpha = 0.3f; // Transparency of the circle


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        Color c = sr.color;
        c.a = circleAlpha; // Make circle semi-transparent
        sr.color = c;
    }

    void Update()
    {
        Color c = sr.color;

        if (key == MusicKey.Idle)
        {
            c.r = 1f;
            c.g = 1f;
            c.b = 1f;
            c.a = circleAlpha;
        }
        else if (key == MusicKey.Yellow)
        {
            c.r = 1f;
            c.g = 1f;
            c.b = 0f;
            c.a = circleAlpha;
        }
        else if (key == MusicKey.Green)
        {
            c.r = 0f;
            c.g = 1f;
            c.b = 0f;
            c.a = circleAlpha;
        }
        else if (key == MusicKey.Blue)
        {
            c.r = 0f;
            c.g = 0.5f;
            c.b = 1f;
            c.a = circleAlpha;
        }
        else if (key == MusicKey.Red)
        {
            c.r = 1f;
            c.g = 0f;
            c.b = 0f;
            c.a = circleAlpha;
        }

        sr.color = c;
    }
}

