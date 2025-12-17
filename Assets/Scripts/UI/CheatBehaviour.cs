using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;
using TMPro;
using static Cheats;


public class CheatBehaviour : MonoBehaviour
{
    public GameObject sceneManager;
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        if (sceneManager.GetComponent<Cheats>().enabled)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                tmp.text = "CHEATING ALREADY?!";
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                tmp.text = "Bet you feel smart huh?";
            }
            else
            {
                if (ghostCheatActive)
                {
                    tmp.text = "Cheater! >:[\nGhost";
                }
                else if (invincibleCheatActive)
                {
                    tmp.text = "Cheater! >:[\nInvincible";
                }
                else
                {
                    tmp.text = "Cheater! >:[";
                }
            }
        }
        else
        {
            tmp.text = "";
        }
    }
}
