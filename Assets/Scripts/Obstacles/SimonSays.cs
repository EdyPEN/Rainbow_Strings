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
    SpriteRenderer spriteRenderer;

    public bool ChallengeInProcess = false;

    public bool FinishPattern1 = false;
    public bool PlayedPattern1 = false;

    public bool FinishPattern2 = false;
    public bool PlayedPattern2 = false;

    public bool ChallengeBeaten;

    [Header("SimonSaysLogic")]
    // in what time bubble shows each color of pattern they play;
    public float timerPerNote;
    // After each pattern player will have some time to play their strings(AFTER EACH RIGHT NOTE TIMER SHOULD UPDATE);
    public float timerReaction;
    public float failDuration = 3f;
    public float failTimer;
    public float successDuration = 3f;
    private float successTimer;
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

        spriteRenderer = GetComponent<SpriteRenderer>();

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

    void Update()
    {
        SuccessTimerLogic();
        TimerLogic();
        ColorLogic();

        //Challenge Bubble!--------------------
        if (Player.GetComponent<PlayerInteractions>().interactButton == true && ChallengeInProcess == false)
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
    void SuccessTimerLogic()
    {
        if (successTimer > 0)
        {
            successTimer -= Time.deltaTime;
            if (successTimer <= 0)
            {
                successTimer = 0;

                // Start showing Pattern 2 AFTER the pink feedback
                combo = 0;
                timerReaction = 0;
                currentPattern = 1;
                currentNote = 0;

                ClearNotesUI();
                timerPerNote = 1f;
                key = MusicKey.Idle;

                Note[0].SetActive(true);
                Note[1].SetActive(false);
                Note[2].SetActive(false);
                Note[3].SetActive(false);
            }
        }
    }
    void TimerLogic()
    {
        // if player fails
        if (failTimer > 0)
        {
            failTimer -= Time.deltaTime;
            if (failTimer <= 0)
            {
                failTimer = 0;

                timerPerNote = 1f;
                currentNote = 0;
                key = MusicKey.Idle;

                Note[0].SetActive(true);
                Note[1].SetActive(false);
                Note[2].SetActive(false);
                Note[3].SetActive(false);
            }
        }
        // Bubble's timer
        if (timerPerNote > 0)
        {
            timerPerNote -= Time.deltaTime;
        }
        else
        {
            timerPerNote = 0;
        }

        // Player's timer
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
        if (failTimer > 0)
        {
            spriteRenderer.color = Color.black;
            return;
        }

        if (successTimer > 0)
        {
            spriteRenderer.color = Color.magenta;
            return;
        }

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

    //Make each note IDLE
    void ClearNotesUI()
    {
        for (int i = 0; i < Note.Length; i++)
        {
            Note[i].GetComponent<Note>().key = MusicKey.Idle;
        }
    }
    void FailRestartCurrentPattern()
    {
        // 3 seconds fail feedback
        failTimer = failDuration;

        combo = 0;
        timerReaction = 0;

        ClearNotesUI();

        Note[0].SetActive(false);
        Note[1].SetActive(false);
        Note[2].SetActive(false);
        Note[3].SetActive(false);

        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            FinishPattern1 = false;
            currentPattern = 0;
            currentNote = 0;
        }
        else if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            // If player fails on 2 pattern -> reload 2ND PATTERN again
            FinishPattern2 = false;
            currentPattern = 1;
            currentNote = 0;
        }

        // Force bubble to neutral during restart
        key = MusicKey.Idle;
    }
    void SimonSaysLogic()
    {
        if (failTimer > 0 || successTimer > 0)
        {
            return;
        }

        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            timerPerNote = 1f;
            Note[0].SetActive(true);
            spriteRenderer.color = Color.white;
        }
        else if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            timerPerNote = 1f;
            Note[0].SetActive(true);
            spriteRenderer.color = Color.white;
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
            currentNote = 0;
            FinishPattern1 = true;
        }
        // Pattern 2 start
        else if (currentNote == 0 && currentPattern == 1 && timerPerNote == 0 && PlayedPattern1 == true && FinishPattern2 == false)
        {
            timerPerNote = 1;
            Note[1].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 1 && currentPattern == 1 && timerPerNote == 0 && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            Note[2].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 2 && currentPattern == 1 && timerPerNote == 0 && PlayedPattern1 == true)
        {
            timerPerNote = 1;
            Note[3].SetActive(true);
            currentNote++;
        }
        else if (currentNote == 3 && currentPattern == 1 && timerPerNote == 0 && PlayedPattern1 == true)
        {
            timerReaction = 2;
            currentNote = 0;
            FinishPattern2 = true;
        }
    }
    void PlayerLogic()
    {
        if (failTimer > 0 || successTimer > 0)
        {
            key = MusicKey.Idle;
            return;
        }
        if (timerReaction < 0)
        {
            FailRestartCurrentPattern();
            return;
        }

        MusicKey inputKey = ColorDisplay.GetComponent<MusicPlay>().key;

        // Same as HiddenPlatform: ignore when Idle
        if (inputKey == MusicKey.Idle)
        {
            return;
        }

        // Decide which pattern is currently being played by the player
        MusicPlay.MusicKey[] activePattern;

        if (FinishPattern1 == true && PlayedPattern1 == false)
        {
            activePattern = pattern1;
        }
        else if (FinishPattern2 == true && PlayedPattern2 == false)
        {
            activePattern = pattern2;
        }
        else
        {
            // Not in a "player input" phase
            return;
        }

        // Safety clamp
        if (combo < 0)
        {
            combo = 0;
        }
        if (combo > 3)
        {
            combo = 3;
        }

        MusicKey expectedKey = activePattern[combo];

        // like in HiddenPlatform -> any wrong key immediately resets progress
        bool isCorrect = false;

        if (combo == 0)
        {
            if (inputKey == expectedKey)
            {
                isCorrect = true;
            }
        }
        else
        {
            // After first correct note, player must continue within reaction time
            if (timerReaction > 0 && inputKey == expectedKey)
            {
                isCorrect = true;
            }
        }

        if (isCorrect)
        {
            // show the correct note on UI
            Note[combo].GetComponent<Note>().key = expectedKey;

            // refresh reaction timer
            timerReaction = 2;

            combo++;

            // Completed 4 notes
            if (combo >= 4)
            {
                combo = 0;

                if (FinishPattern1 == true && PlayedPattern1 == false)
                {
                    PlayedPattern1 = true;
                    successTimer = successDuration;
                    ClearNotesUI();
                    Note[0].SetActive(false);
                    Note[1].SetActive(false);
                    Note[2].SetActive(false);
                    Note[3].SetActive(false);

                    currentPattern++;
                    timerPerNote = 1;
                    key = MusicKey.Idle;
                }
                else if (FinishPattern2 == true && PlayedPattern2 == false)
                {
                    PlayedPattern2 = true;
                    timerPerNote = 1;
                }
            }

            return;
        }

        FailRestartCurrentPattern();
    }
}