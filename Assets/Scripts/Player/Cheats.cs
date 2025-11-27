using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public bool isInvincible;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInvincible = !isInvincible;
        }
    }
}
