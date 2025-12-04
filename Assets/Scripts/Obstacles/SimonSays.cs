using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static MusicPlay;

public class SimonSays : MonoBehaviour
{
    public GameObject Player;
    public GameObject ColorDisplay;

    public GameObject[] Note;

    //public Sprite Idle, Blue, Green, Yellow, Red; ---- We will need this for diffrent sprites of our character.
    SpriteRenderer sr;

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

    public MusicPlay.MusicKey[] pattern1 = new MusicPlay.MusicKey[4];
    public MusicPlay.MusicKey[] pattern2 = new MusicPlay.MusicKey[4];
    public MusicKey key;

    public int currentNote = 0;
    public int currentPattern = 0;

    void Start()
    {
        PatternRandomizer(pattern1);
        PatternRandomizer(pattern2);

        Note[0].SetActive(false);
        Note[1].SetActive(false);
        Note[2].SetActive(false);
        Note[3].SetActive(false);

        sr = GetComponent<SpriteRenderer>();

        ChallengeBeaten = false;
    }
    void PatternRandomizer(MusicPlay.MusicKey[] pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            int[] keys = (int[])Enum.GetValues(typeof(MusicPlay.MusicKey));
            int minKey = Mathf.Min(keys) + 1;
            int maxKey = Mathf.Max(keys);

            int newKey = UnityEngine.Random.Range(minKey, maxKey + 1);

            if (i != 0)
            {
                if (newKey == (int)pattern[i - 1])
                {
                    ++newKey;
                    if (newKey > maxKey)
                        newKey = minKey;
                }
            }
            pattern[i] = (MusicPlay.MusicKey)newKey;
        }
    }
    // Update is called once per frame
    void Update()
    {
        TimerLogic();
        ColorLogic();

        //Challenge Bubble!--------------------
        if (Player.GetComponent<PlayerInteractions>().interactButton == true)
        {
            timerPerNote = 1;
            Note[0].SetActive(true);
            ChallengeInProcess = true;
        }

        if (ChallengeInProcess == true)
        {
            SimonSaysLogic();
        }
        //Challenge Bubble!--------------------


        if (FinishPattern1 == true && PlayedPattern1 == false || (FinishPattern2 == true && PlayedPattern2 == false))
        {
            PlayerLogic();
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
    void TimerLogic()
    {
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
    }
    void ColorLogic()
    {
        if (key == MusicKey.Idle)
        {
            sr.color = Color.white;
        }
        else if (key == MusicKey.Yellow)
        {
            sr.color = Color.yellow;
        }
        else if (key == MusicKey.Green)
        {
            sr.color = Color.green;
        }
        else if (key == MusicKey.Blue)
        {
            sr.color = Color.deepSkyBlue;
        }
        else if (key == MusicKey.Red)
        {
            sr.color = Color.red;
        }
    }
    void SimonSaysLogic()
    {
        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            key = MusicKey.Idle;
            return;
        }

        if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            key = MusicKey.Idle;

            return;
        }

        // missing check of the player INSIDE the area
        if (currentPattern == 0)
        {
            key = pattern1[currentNote];
        } 
        else if (currentPattern == 1)
        {
            key = pattern2[currentNote];
        }
        
        // Pattern 1 start
        if (currentNote == 0 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            Note[1].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 1 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            Note[2].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 2 && currentPattern == 0 && timerPerNote == 0)
        {
            timerPerNote = 1;
            Note[3].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 3 && currentPattern == 0 && timerPerNote == 0)
        {
            timerReaction = 2;
            currentPattern++;
            currentNote = 0;
            FinishPattern1 = true;
        }
        // Pattern 2 start
        else if (currentNote == 0 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true && FinishPattern2 == false)
        {
            timerPerNote = 1;
            Note[1].GetComponent<Note>().key = MusicKey.Idle;
            currentNote++;
        }
        else if (currentNote == 1 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            Note[2].GetComponent<Note>().key = MusicKey.Idle;
            currentNote++;
        }
        else if (currentNote == 2 && currentPattern == 1 && timerPerNote == 0 && FinishPattern1 == true && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            Note[3].GetComponent<Note>().key = MusicKey.Idle;
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
        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Yellow) && (combo == 0))
            {
                combo = 1;
                timerReaction = 2;
                Note[0].GetComponent<Note>().key = MusicKey.Yellow;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Green) && (timerReaction > 0) && (combo == 1))
            {
                combo = 2;
                timerReaction = 2;
                Note[1].GetComponent<Note>().key = MusicKey.Green;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Red) && (timerReaction > 0) && (combo == 2))
            {
                combo = 3;
                timerReaction = 2;
                Note[2].GetComponent<Note>().key = MusicKey.Red;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Blue) && (timerReaction > 0) && (combo == 3))
            {
                PlayedPattern1 = true;
                combo = 0;
                timerPerNote = 1;
                Note[3].GetComponent<Note>().key = MusicKey.Blue;
                Note[0].GetComponent<Note>().key = MusicKey.Idle; // This just cancels the previous line D:
            }
            else if (timerReaction == 0)
            {
                combo = 0;
                FinishPattern1 = false;
                currentPattern = 0;
                currentNote = 0;
                timerPerNote = 1;
                Note[1].SetActive(false);
                Note[2].SetActive(false);
                Note[3].SetActive(false);
            }
        }
        //Pattern2
        if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Red) && (combo == 0))
            {
                combo = 1;
                Note[0].GetComponent<Note>().key = MusicKey.Red;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Green) && (timerReaction > 0) && (combo == 1))
            {
                combo = 2;
                Note[1].GetComponent<Note>().key = MusicKey.Green;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Blue) && (timerReaction > 0) && (combo == 2))
            {
                combo = 3;
                Note[2].GetComponent<Note>().key = MusicKey.Blue;
                timerReaction = 2;
            }
            else if ((ColorDisplay.GetComponent<MusicPlay>().key == MusicKey.Green) && (timerReaction > 0) && (combo == 3))
            {
                PlayedPattern2 = true;
                combo = 0;
                Note[3].GetComponent<Note>().key = MusicKey.Green;
                timerPerNote = 1;
            }
            else if (timerReaction == 0)
            {
                combo = 0;
                FinishPattern2 = false;
                currentPattern = 1;
                currentNote = 0;
                timerPerNote = 1;
                Note[1].SetActive(false);
                Note[2].SetActive(false);
                Note[3].SetActive(false);
            }
        }

    }
}
