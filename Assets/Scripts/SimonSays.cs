using UnityEngine;

public class SimonSays : MonoBehaviour
{
    public GameObject Player;
    public GameObject ColorDisplay;

    //public Sprite Idle, Blue, Green, Yellow, Red; ---- We will need this for diffrent sprites of our character.
    SpriteRenderer sr;
    public int spriteState = 0;

    private int idle = 0;
    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    public bool ChallengeInProcess = false;
    public bool ChallengeBeaten;

    [Header("SimonSaysLogic")]
    // in what time bubble/fubble shows each color of pattern they play;
    public float timerPerNote;
    // After each pattern player will have some time to play their strings(AFTER EACH RIGHT NOTE TIMER SHOULD UPDATE);
    public float timerReaction;

    public int playerNote;

    public int[] pattern1 = new int[4];
    public int[] pattern2 = new int[4];
    public int currentNote = 0;
    public int currentPattern = 0;


    void Start()
    {
        pattern1[0] = yellow;
        pattern1[1] = green;
        pattern1[2] = red;
        pattern1[3] = blue;

        pattern2[0] = red;
        pattern2[1] = green;
        pattern2[2] = blue;
        pattern2[3] = yellow;

        sr = GetComponent<SpriteRenderer>();

        //Same as in HiddenPlatforms, but with +1 note and 2 patterns
        //for (int i = 0; i < pattern1.Length; i++)
        //{
        //    pattern1[i] = i + 1;
        //}
        //spriteState = pattern1[0];

        ChallengeBeaten = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerPerNote > 0)
        {
            timerPerNote -= Time.deltaTime;
        }
        else
        {
            timerPerNote = 0;
        }

        ColorLogic();
        if (Player.GetComponent<PlayerInteractions>().interactButton == true)
        {
            timerPerNote = 1;
            ChallengeInProcess = true;
        }
        if (ChallengeInProcess == true)
        {
            SimonSaysLogic();
        }
        if (ChallengeBeaten == true)
        {
            Player.GetComponentInParent<PlayerInteractions>().keyCollected = true;
        }
    }

    void ColorLogic()
    {
        if (spriteState == idle)
        {
            sr.color = Color.blue;
        }
        else if (spriteState == yellow)
        {
            sr.color = Color.yellow;
        }
        else if (spriteState == green)
        {
            sr.color = Color.green;
        }
        else if (spriteState == blue)
        {
            sr.color = Color.deepSkyBlue;
        }
        else if (spriteState == red)
        {
            sr.color = Color.red;
        }
    }

    void SimonSaysLogic()
    {
        // missing check of the player INSIDE the area
        if (currentPattern == 0)
        {
            spriteState = pattern1[currentNote];
        } else if (currentPattern == 1)
        {
            spriteState = pattern2[currentNote];
        }

        if (currentNote == 0 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 1 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 2 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 3 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            currentPattern++;
            spriteState = 0;
            currentNote = 0;
            //PlayerLogic();
        }
        else if (currentNote == 0 && currentPattern == 1 && timerPerNote == 0)
        {

        }
    }

}
