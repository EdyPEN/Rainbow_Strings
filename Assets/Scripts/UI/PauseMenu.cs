using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseContainer;

    public GameObject[] pauseButtons;

    public GameObject optionsContainer;

    public GameObject[] optionsButtons;

    public GameObject volumeContainer;

    public GameObject[] volumeButtons;

    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;

    public GameObject musicUpArrow;
    public GameObject musicDownArrow;

    public GameObject sfxUpArrow;
    public GameObject sfxDownArrow;

    public bool pauseInput, confirmInput, cancelInput;

    public static bool isPaused;

    public static int musicVolume = 10;

    public static int sfxVolume = 10;

    public int selectedMenu; // 1 paused, 2 options, 3 volume

    public int selectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseManager();

        PauseButton();

        NavigateMenu();

        MenuHighlights();

        Inputs();

        MenuSelection();

        VolumeChange();

        VolumeVisuals();
    }

    void PauseManager()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            if (selectedMenu == 1)
            {
                optionsContainer.SetActive(false);
                pauseContainer.SetActive(true);
                volumeContainer.SetActive(false);
            }
            else if (selectedMenu == 2)
            {
                pauseContainer.SetActive(false);
                optionsContainer.SetActive(true);
                volumeContainer.SetActive(false);
            }
            else if (selectedMenu == 3)
            {
                optionsContainer.SetActive(false);
                pauseContainer.SetActive(false);
                volumeContainer.SetActive(true);
            }
        }
        else 
        {
            pauseContainer.SetActive(false);
            optionsContainer.SetActive(false);
            volumeContainer.SetActive(false);
            Time.timeScale = 1;
            selectedButton = 1;
            selectedMenu = 1;
        }
    }

    void PauseButton()
    {
        if (pauseInput)
        {
            isPaused = !isPaused;
        }
        if (cancelInput && isPaused)
        {
            if (selectedMenu == 1)
            {
                isPaused = false;
            }
            else if (selectedMenu == 2)
            {
                selectedMenu = 1;
                selectedButton = 1;
            }
            else if (selectedMenu == 3)
            {
                selectedMenu = 2;
                selectedButton = 1;
            }
        }
    }

    void Inputs()
    {
        pauseInput = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || (Input.GetKeyDown(KeyCode.Return) && !isPaused);
        confirmInput = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space);
        cancelInput = (Input.GetKeyDown(KeyCode.Escape) && isPaused) || Input.GetKeyDown(KeyCode.Backspace);
    }

    void NavigateMenu()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (selectedButton > 0)
            {
                selectedButton--;
            }
            else
            {
                selectedButton = 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (selectedButton < 2)
            {
                selectedButton++;
            }
            else
            {
                selectedButton = 0;
            }
        }
    }

    void MenuHighlights()
    {
        for (int i = 0; i < pauseButtons.Length; i++)
        {
            if (selectedMenu == 1)
            {
                if (i == selectedButton)
                {
                    pauseButtons[i].transform.localScale = new Vector3(1, 1, 1);
                    pauseButtons[i].GetComponent<Image>().color = Color.deepSkyBlue;
                }
                else
                {
                    pauseButtons[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    pauseButtons[i].GetComponent<Image>().color = Color.cornflowerBlue;
                }
            }
            else if (selectedMenu == 2)
            {
                if (i == selectedButton)
                {
                    optionsButtons[i].transform.localScale = new Vector3(1, 1, 1);
                    optionsButtons[i].GetComponent<Image>().color = Color.deepSkyBlue;
                }
                else
                {
                    optionsButtons[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    optionsButtons[i].GetComponent<Image>().color = Color.cornflowerBlue;
                }
            }
            else if (selectedMenu == 3)
            {
                if (i == selectedButton)
                {
                    volumeButtons[i].transform.localScale = new Vector3(1, 1, 1);
                    volumeButtons[i].GetComponent<Image>().color = Color.deepSkyBlue;
                }
                else
                {
                    volumeButtons[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    volumeButtons[i].GetComponent<Image>().color = Color.cornflowerBlue;
                }
            }
        }
    }

    void MenuSelection()
    {
        if (!isPaused || !confirmInput)
        {
            return;
        }
        if (selectedMenu == 1)
        {
            if (selectedButton == 0)
            {
                selectedMenu = 2;
                selectedButton = 1;
            }
            else if (selectedButton == 1)
            {
                isPaused = false;
            }
            else if (selectedButton == 2)
            {
                isPaused = false;
                SceneManager.LoadScene(0);
            }
        }
        else if (selectedMenu == 2)
        {
            if (selectedButton == 0)
            {
                selectedMenu = 3;
                selectedButton = 1;
            }
            else if (selectedButton == 1)
            {
                selectedMenu = 1;
                selectedButton = 1;
            }
            else if (selectedButton == 2)
            {
                //Connection
            }
        }
        else if (selectedMenu == 3)
        {
            if (selectedButton == 1)
            {
                selectedMenu = 2;
                selectedButton = 1;
            }
        }
    }
    void VolumeChange()
    {
        if (selectedMenu != 3)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (selectedButton == 0 && musicVolume < 10)
            {
                musicVolume++;
            }
            else if (selectedButton == 2 && sfxVolume < 10)
            {
                sfxVolume++;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (selectedButton == 0 && musicVolume > 0)
            {
                musicVolume--;
            }
            else if (selectedButton == 2 && sfxVolume > 0)
            {
                sfxVolume--;
            }
        }
    }

    void VolumeVisuals()
    {
        musicText.text = "Music: " + musicVolume;
        sfxText.text = "SFX: " + sfxVolume;

        if (selectedMenu != 3)
        {
            return;
        }
        if (selectedButton == 0)
        {
            if (musicVolume == 10)
            {
                musicUpArrow.SetActive(false);
                musicDownArrow.SetActive(true);
            }
            else if (musicVolume == 0)
            {
                musicUpArrow.SetActive(true);
                musicDownArrow.SetActive(false);
            }
            else
            {
                musicUpArrow.SetActive(true);
                musicDownArrow.SetActive(true);
            }
        }
        else
        {
            musicUpArrow.SetActive(false);
            musicDownArrow.SetActive(false);
        }

        if (selectedButton == 2)
        {
            if (sfxVolume == 10)
            {
                sfxUpArrow.SetActive(false);
                sfxDownArrow.SetActive(true);
            }
            else if (sfxVolume == 0)
            {
                sfxUpArrow.SetActive(true);
                sfxDownArrow.SetActive(false);
            }
            else
            {
                sfxUpArrow.SetActive(true);
                sfxDownArrow.SetActive(true);
            }
        }
        else
        {
            sfxUpArrow.SetActive(false);
            sfxDownArrow.SetActive(false);
        }
    }
}
