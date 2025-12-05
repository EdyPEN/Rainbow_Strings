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

    public static Vector3 respawnPosition = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = respawnPosition;
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
        if (collision.gameObject.CompareTag("Ripple"))
        {
            respawnPosition = collision.gameObject.transform.position;
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
        if (!GetComponent<Cheats>().invincibleCheatActive && !GetComponentInChildren<PlayerMovement>().playerIsInvincible)
        {
            hp -= damage;
            GetComponentInChildren<PlayerMovement>().playerIsStunned = true;
            rb.linearVelocity = new Vector2(-GetComponentInChildren<PlayerMovement>().playerFacingDirection *  horizontalDamageKnockback, verticalDamageKnockback);
        }
    }
} 
