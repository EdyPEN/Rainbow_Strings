using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static PauseMenu;

public class MusicPlay : MonoBehaviour
{
    public GameObject AreaInteraction;

    //public float timer;

    public enum MusicKey { Idle, Blue, Green, Yellow, Red }

    SpriteRenderer spriteRenderer;

    public MusicKey key;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //if (timer > 0)
        //{
        //    timer -= Time.deltaTime;
        //}
        //else
        //{
        //    timer = 0;
        //}

        if (isPaused)
        {
            return;
        }
        ColorChange();

        NotePressed();
    }

    public void NotePressed()
    {
        //if (timer >= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                key = MusicKey.Blue;
                //timer = 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                key = MusicKey.Green;
                //timer = 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                key = MusicKey.Yellow;
                //timer = 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                key = MusicKey.Red;
                //timer = 0.1f;
            }
            else /*if (timer == 0)*/
            {
                key = MusicKey.Idle;
            }
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
