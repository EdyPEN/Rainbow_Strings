using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    Rigidbody2D keyRigidBody;
    Rigidbody2D playerRigidBody;
    PlayerMovement playerMovement;
    PlayerInteractions playerInteractions;

    public GameObject player;

    public Vector2 offset;
    public Vector2 target;
    public Vector2 acceleration;
    public Vector2 predictionMultiplier;
    public Vector2 speedModifierDistance;

    public float speedDefault;

    public float precision;

    void Start()
    {
        keyRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        playerInteractions = player.GetComponent<PlayerInteractions>();
        playerMovement = player.GetComponentInChildren<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (playerInteractions.keyCollected)
        {
            target.x = player.transform.position.x + playerRigidBody.linearVelocityX * predictionMultiplier.x + (offset.x * playerMovement.playerFacingDirection);

            target.y = player.transform.position.y + playerRigidBody.linearVelocityY * predictionMultiplier.y + offset.y;

            speedModifierDistance = target - (Vector2)transform.position;

            acceleration = speedModifierDistance * speedDefault - keyRigidBody.linearVelocity * precision;

            keyRigidBody.linearVelocity += acceleration * Time.fixedDeltaTime;
        }

        if (playerInteractions.gateOpened)
        {
            gameObject.SetActive(false);
        }
    }
}