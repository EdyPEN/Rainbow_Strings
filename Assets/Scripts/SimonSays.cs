using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UIElements;

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

    public bool FinishPattern1 = false;
    public bool PlayedPattern1 = false;

    public bool FinishPattern2 = false;
    public bool PlayedPattern2 = false;

    public bool ChallengeBeaten;

    [Header("SimonSaysLogic")]
    // in what time bubble/fubble shows each color of pattern they play;
    public float timerPerNote;
    // After each pattern player will have some time to play their strings(AFTER EACH RIGHT NOTE TIMER SHOULD UPDATE);
    public float timerReaction;
    public int combo = 0;

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
        pattern2[3] = green;

        sr = GetComponent<SpriteRenderer>();

        ChallengeBeaten = false;
    }

    // Update is called once per frame
    void Update()
    {
        // TIMER------------------
        if (timerPerNote > 0)
        {
            timerPerNote -= Time.deltaTime;
        }
        else
        {
            timerPerNote = 0;
        }

        if (timerReaction > 0)
        {
            timerReaction -= Time.deltaTime;
        }
        else
        {
            timerReaction = 0;
        }
        // TIMER------------------


        ColorLogic();

        //Challenge Bubble!--------------------
        if (Player.GetComponent<PlayerInteractions>().interactButton == true)
        {
            timerPerNote = 1;
            ChallengeInProcess = true;
        }

        if (ChallengeInProcess == true)
        {
            SimonSaysLogic();
        }
        //Challenge Bubble!--------------------


        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            PlayerLogic(); // check player input every frame
        }

        if (PlayedPattern2 == true)
        {
            ChallengeBeaten = true;
        }

        if (ChallengeBeaten == true)
        {
            Player.GetComponentInParent<PlayerInteractions>().keyCollected = true;
            Destroy(gameObject);
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
        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            spriteState = idle;
            return;
        }

        if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            spriteState = idle;
            return;
        }

        // missing check of the player INSIDE the area
        if (currentPattern == 0)
        {
            spriteState = pattern1[currentNote];
        } 
        else if (currentPattern == 1)
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
            timerReaction = 2;
            currentPattern++;
            currentNote = 0;
            FinishPattern1 = true;
        } 
        else if (currentNote == 0 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true && FinishPattern2 == false)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 1 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 2 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            currentNote++;
        }
        else if (currentNote == 3 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true)
        {
            timerReaction = 2;
            currentNote = 0;
            FinishPattern2 = true;
        }
    }

    void PlayerLogic()
    {
        // Same code as Skips
        if (FinishPattern1 == true)
        {
            if ((ColorDisplay.GetComponent<MusicPlay>().color == yellow) && (combo >= 0))
            {
                combo = 1;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == green) && (timerReaction > 0) && (combo >= 1))
            {
                combo = 2;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == red) && (timerReaction > 0) && (combo >= 2))
            {
                combo = 3;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == blue) && (timerReaction > 0) && (combo >= 3))
            {
                PlayedPattern1 = true;
                combo = 0;
            }
            else if (timerReaction == 0)
            {
                combo = 0;
                FinishPattern1 = false;
                currentPattern = 0;
                currentNote = 0;
                timerPerNote = 1;
            }
        }

        if (FinishPattern2 == true)
        {
            if ((ColorDisplay.GetComponent<MusicPlay>().color == red) && (combo >= 0))
            {
                combo = 1;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == green) && (timerReaction > 0) && (combo >= 1))
            {
                combo = 2;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == blue) && (timerReaction > 0) && (combo >= 2))
            {
                combo = 3;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().color == green) && (timerReaction > 0) && (combo >= 3))
            {
                PlayedPattern2 = true;
            }
            else if (timerReaction == 0)
            {
                combo = 0;
                FinishPattern1 = false;
                currentPattern = 0;
                currentNote = 0;
                timerPerNote = 1;
            }
        }

    }

}
