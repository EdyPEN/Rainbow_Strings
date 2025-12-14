using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    public GameObject[] buttons;

    public bool pauseInput, confirmInput, cancelInput;

    public static bool isPaused;

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
    }

    void PauseManager()
    {
        if (isPaused)
        {
            container.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            container.SetActive(false);
            Time.timeScale = 1;
            selectedButton = 1;
        }
    }

    void PauseButton()
    {
        if (pauseInput)
        {
            isPaused = !isPaused;
        }
    }

    void Inputs()
    {
        pauseInput = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || (Input.GetKeyDown(KeyCode.Return) && !isPaused);
        confirmInput = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space);
        cancelInput = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace);
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
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedButton)
            {
                buttons[i].transform.localScale = new Vector3(1, 1, 1);
                buttons[i].GetComponent<Image>().color = Color.deepSkyBlue;
            }
            else
            {
                buttons[i].transform.localScale = new Vector3(0.5f, 0.5f, 1);
                buttons[i].GetComponent<Image>().color = Color.cornflowerBlue;
            }
        }
    }

    void MenuSelection()
    {
        if (!isPaused || !confirmInput)
        {
            return;
        }
        if (selectedButton == 0)
        {
            //Options
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
}
