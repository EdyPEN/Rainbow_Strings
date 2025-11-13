using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public int hp;
    public bool keyCollected = false;
    public bool inChallengeArea = false;
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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Challenge")
        {
            inChallengeArea = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lock")
        {
            if (keyCollected == true)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
