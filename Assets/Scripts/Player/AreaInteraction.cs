using UnityEngine;

public class AreaInteraction : MonoBehaviour
{
    public MusicPlay musicPlay;

    SpriteRenderer sr;

    public float maxScale = 9f;      // Max size of the circle
    public float startScale = 0.2f;  // Start size when circle appears
    public float growSpeed = 6f;     // How fast circle grows
    public float circleAlpha = 0.4f; // Transparency of the circle

    private bool isGrowing = false;  // Is circle growing now
    private float currentScale = 0f; // Current scale
    private int waveColor = 0;       // Color of current wave (1-4)

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        HideCircle();
    }

    void Update()
    {
        int musicColor = musicPlay.color;

        // If some note is active and no wave now -> start new wave
        if (!isGrowing && musicColor != 0)
        {
            StartWave(musicColor);
        }

        // If wave is active -> grow the circle
        if (isGrowing)
        {
            currentScale += growSpeed * Time.deltaTime;

            if (currentScale >= maxScale)
            {
                StopWave(); // When max size reached -> hide circle
            }
            else
            {
                transform.localScale = new Vector3(currentScale, currentScale, 1f);
            }
        }
    }

    void StartWave(int noteColor)
    {
        isGrowing = true;
        waveColor = noteColor;

        currentScale = startScale;
        transform.localScale = new Vector3(currentScale, currentScale, 1f);

        ShowCircle();
        UpdateCircleColor(); // Set color according to note
    }

    void StopWave()
    {
        isGrowing = false;
        HideCircle();
    }

    void ShowCircle()
    {
        if (sr == null)
        {
            return;
        }

        Color c = sr.color;
        c.a = circleAlpha; // Make circle semi-transparent
        sr.color = c;
    }

    void HideCircle()
    {
        if (sr == null)
        {
            return;
        }

        // Hide by scaling to zero and removing alpha
        transform.localScale = Vector3.zero;

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
    }

    void UpdateCircleColor()
    {
        if (sr == null)
        {
            return;
        }

        Color c = sr.color;

        // Change RGB, keep current alpha
        if (waveColor == 1)
        {
            c.r = 0f;
            c.g = 0.5f;
            c.b = 1f;
        }
        else if (waveColor == 2)
        {
            c.r = 0f;
            c.g = 1f;
            c.b = 0f;
        }
        else if (waveColor == 3)
        {
            c.r = 1f;
            c.g = 1f;
            c.b = 0f;
        }
        else if (waveColor == 4)
        {
            c.r = 1f;
            c.g = 0f;
            c.b = 0f;
        }

        sr.color = c;
    }
}

