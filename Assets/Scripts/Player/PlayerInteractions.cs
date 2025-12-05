using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInteractions : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private PlayerMovement playerMovement;
    private Cheats cheats;

    public int hp;
    public bool keyCollected;
    public bool gateOpened;

    public bool inChallengeArea;
    public bool interactButton;

    public float horizontalDamageKnockback, verticalDamageKnockback;

    public static Vector3 respawnPosition = Vector3.zero;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        playerMovement = GetComponentInChildren<PlayerMovement>();

        cheats = GetComponent<Cheats>();

        transform.position = respawnPosition;
    }

    private void Update()
    {
        interactButton = Input.GetKeyDown(KeyCode.E);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Respawn();
    }

    void Respawn()
    {
        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InsideBubbleChallengeRange(collision);

        MetRipple(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OutsideBubbleChallengeRange(collision);
    }

    void InsideBubbleChallengeRange(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Challenge"))
        {
            inChallengeArea = true;
        }
    }

    void MetRipple(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ripple"))
        {
            respawnPosition = collision.gameObject.transform.position;
        }
    }

    void OutsideBubbleChallengeRange(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Challenge"))
        {
            inChallengeArea = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UnlockingGate(collision);
    }

    void UnlockingGate(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gate"))
        {
            if (!keyCollected)
            {
                return;
            }
            gateOpened = true;
            collision.gameObject.SetActive(false);
        }
    }

    // Other Functions
    public void TakeDamage(int damage)
    {
        if (!cheats.invincibleCheatActive && !playerMovement.playerIsInvincible)
        {
            hp -= damage;
            playerMovement.playerIsStunned = true;
            playerRigidBody.linearVelocity = new Vector2(-playerMovement.playerFacingDirection *  horizontalDamageKnockback, verticalDamageKnockback);
        }
    }
} 
