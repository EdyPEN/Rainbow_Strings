using UnityEngine;
using static PlayerInteractions;
using static PauseMenu;

public class CameraBehaviour : MonoBehaviour
{
    PlayerMovement playerMovement;

    PlayerInteractions playerInteractions;

    public GameObject player;

    public Vector2 target;

    public float offsetX, offsetYLimit, manualOffsetY;

    public float speedDefault, speedModifierDistance;

    public bool playerDead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = player.GetComponentInChildren<PlayerMovement>();

        playerInteractions = player.GetComponent<PlayerInteractions>();

        transform.position = new Vector3(respawnPosition.x, respawnPosition.y + manualOffsetY, respawnPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInteractions.playerDead || isPaused)
        {
            return;
        }

        target.x = player.transform.position.x + (offsetX * playerMovement.playerFacingDirection);

        if (Mathf.Abs(target.y - (player.transform.position.y + manualOffsetY)) > offsetYLimit)
        {
            if (target.y < (player.transform.position.y + manualOffsetY))
            {
                target.y = (player.transform.position.y + manualOffsetY) - offsetYLimit;
            }
            else
            {
                target.y = (player.transform.position.y + manualOffsetY) + offsetYLimit;
            }
        }

        speedModifierDistance = Vector2.Distance(transform.position, target);

        Vector3 currentPosition2D = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 targetPosition2D = new Vector3(target.x, target.y, -10);

        transform.position = Vector3.MoveTowards(currentPosition2D, targetPosition2D, speedDefault * speedModifierDistance * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, target.y, -10);
    }
}
