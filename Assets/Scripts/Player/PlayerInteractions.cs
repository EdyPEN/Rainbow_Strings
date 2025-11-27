using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInteractions : MonoBehaviour
{
    Rigidbody2D rb;

    public int hp;
    public bool keyCollected;
    public bool gateOpened;

    public bool inChallengeArea;
    public bool interactButton;

    public float horizontalDamageKnockback, verticalDamageKnockback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        interactButton = Input.GetKeyDown(KeyCode.E);

        if (interactButton == true && inChallengeArea == true)
        {
            //keyCollected = true;
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

    public void TakeDamage(int damage)
    {
        if (GetComponent<Cheats>().isInvincible == false)
        {
            hp -= damage;
            GetComponent<PlayerMovement>().ignorePlayerMovement = true;
            rb.linearVelocity = new Vector2(-GetComponent<PlayerMovement>().playerFacingDirection *  horizontalDamageKnockback, verticalDamageKnockback);
        }
    }
} 
