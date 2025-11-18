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

    public bool ChallengeBeaten;

    [Header("SimonSaysLogic")]
    // in what time bubble/fubble shows each color of pattern they play;
    public float timerPerNote;
    // After each pattern player will have some time to play their strings(AFTER EACH RIGHT NOTE TIMER SHOULD UPDATE);
    public float timerReaction;

    public int currentNote = 0;
    public int currentPattern = 0;
    public int playerNote;

    public int[] pattern1 = new int[4];
    public int[] pattern2 = new int[4];


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        //Same as in HiddenPlatforms, but with +1 note and 2 patterns
        for (int i = 0; i < pattern1.Length; i++)
        {
            pattern1[i] = i + 1;
        }
        spriteState = pattern1[0];

        //for (int i = 0; i < pattern2.Length; i++)
        //{
        //    pattern2[i] = i + 1;
        //}
        //spriteState = pattern2[0];


        ChallengeBeaten = false;
    }

    // Update is called once per frame
    void Update()
    {
        //playerNote = ColorDisplay.GetComponent<MusicPlay>().color;

        //if (playerNote == pattern1[currentNote])
        //{
        //    currentNote++;
        //}

        //if (currentNote < 4)
        //{
        //    spriteState = pattern1[currentNote];
        //}

        ColorLogic();
        if (Player.GetComponent<PlayerInteractions>().interactButton == true)
        {
            SimonSaysLogic();
            if (ChallengeBeaten == true)
            {
                Player.GetComponentInParent<PlayerInteractions>().keyCollected = true;
            }
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

    }

}
