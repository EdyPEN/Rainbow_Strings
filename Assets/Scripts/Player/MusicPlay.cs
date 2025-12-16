using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static PauseMenu;

public class MusicPlay : MonoBehaviour
{
    public GameObject AreaInteraction;
    AudioManager audioManager;

    public enum MusicKey { Idle, Blue, Green, Yellow, Red }

    SpriteRenderer spriteRenderer;

    public MusicKey key;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (isPaused)
        {
            return;
        }
        ColorChange();

        NotePressed();
    }

    public void NotePressed()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            key = MusicKey.Blue;
            audioManager.PlaySFX(audioManager.PlayerNoteBlue);
            //timer = 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            key = MusicKey.Green;
            audioManager.PlaySFX(audioManager.PlayerNoteGreen);
            //timer = 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            key = MusicKey.Yellow;
            audioManager.PlaySFX(audioManager.PlayerNoteYellow);
            //timer = 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            key = MusicKey.Red;
            audioManager.PlaySFX(audioManager.PlayerNoteRed);
            //timer = 0.1f;
        }
        else /*if (timer == 0)*/
        {
            key = MusicKey.Idle;
        }
    }

    private void ColorChange()
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
}
