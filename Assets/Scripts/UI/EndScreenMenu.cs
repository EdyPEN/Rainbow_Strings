using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenMenu : MonoBehaviour
{
    public GameObject container;

    public GameObject mainMenuButton;

    public bool confirmInput;

    public int selectedButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MenuHighlights();

        Inputs();

        MenuSelection();
    }

    void Inputs()
    {
        confirmInput = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space);
    }

    void MenuHighlights()
    {
        mainMenuButton.transform.localScale = new Vector3(1, 1, 1);
        mainMenuButton.GetComponent<Image>().color = Color.deepSkyBlue;
    }

    void MenuSelection()
    {
        if (!confirmInput)
        {
            return;
        }
        if (selectedButton == 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}
