using UnityEngine;
using UnityEngine.SceneManagement;
using static Cheats;
public class SceneManagement : MonoBehaviour
{
    Cheats cheats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cheats = GetComponent<Cheats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            invincibleCheatActive = false;
            ghostCheatActive = false;
            cheats.enabled = !cheats.enabled;
        }
    }
}
