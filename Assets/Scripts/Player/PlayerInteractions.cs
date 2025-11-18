using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractions : MonoBehaviour
{
    public int hp;
    public bool keyCollected;
    public bool gateOpened;

    public bool inChallengeArea;
    public bool interactButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        interactButton = Input.GetKeyDown(KeyCode.E);

        if (interactButton == true && inChallengeArea == true)
        {
            keyCollected = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (keyCollected == true)
        {
            //Destroy(GameObject.FindGameObjectWithTag("Key"));
        }

        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Challenge"))
        {
            inChallengeArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Challenge"))
        {
            inChallengeArea = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            if (keyCollected == true)
            {
                gateOpened = true;
                Destroy(collision.gameObject);
            }
        }
    }
}
