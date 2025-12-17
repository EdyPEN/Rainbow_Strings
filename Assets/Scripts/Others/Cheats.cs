using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerInteractions;
using static PauseMenu;

public class Cheats : MonoBehaviour
{
    PlayerInteractions playerInteractions;

    public static bool invincibleCheatActive;

    public static bool ghostCheatActive;

    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
        {
            playerInteractions = player.GetComponent<PlayerInteractions>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.I) && !ghostCheatActive)
        {
            invincibleCheatActive = !invincibleCheatActive;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ghostCheatActive = !ghostCheatActive;
            invincibleCheatActive = ghostCheatActive;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            playerInteractions.keyCollected = true;
        }

        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.N) && SceneManager.GetActiveScene().buildIndex < 3)
        {
            respawnPosition = Vector3.zero;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().buildIndex > 0)
        {
            respawnPosition = Vector3.zero;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
