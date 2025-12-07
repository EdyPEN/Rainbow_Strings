using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static PlayerMovement;

public class PlayerInteractions : MonoBehaviour
{
    private Cheats cheats;
    private Rigidbody2D playerRigidBody;
    private PlayerMovement playerMovement;

    public int hp;
    public int maxHp;
    public bool gateOpened;
    public bool keyCollected;

    public bool interactButton;
    public bool inChallengeArea;

    public float horizontalDamageKnockback;
    public float verticalDamageKnockback;

    public static Vector3 respawnPosition = new Vector3(-3, 8, 0);

    void Start()
    {
        cheats = GetComponent<Cheats>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponentInChildren<PlayerMovement>();

        hp = maxHp;

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
            hp = maxHp;

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

    // Other Functions
    public void TakeDamage(int damage, bool useContactDirection, Collision2D collision)
    {
        if (!cheats.invincibleCheatActive && !playerMovement.playerIsInvincible)
        {
            hp -= damage;
            playerMovement.playerIsStunned = true;
            if (!useContactDirection)
            {
                playerRigidBody.linearVelocity = new Vector2(-playerMovement.playerFacingDirection * horizontalDamageKnockback, verticalDamageKnockback);
            }
            else
            {
                int pushDirection = playerMovement.playerFacingDirection;

                if (GetCollisionNormal(collision).x > 0)
                {
                    pushDirection = Mathf.CeilToInt(GetCollisionNormal(collision).x);
                }
                else if (GetCollisionNormal(collision).x < 0)
                {
                    pushDirection = Mathf.FloorToInt(GetCollisionNormal(collision).x);
                }

                playerRigidBody.linearVelocity = new Vector2(-pushDirection * horizontalDamageKnockback, verticalDamageKnockback);
            }
        }
    }
} 
