using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;

    public Vector2 target;

    public float offsetX, offsetYLimit, manualOffsetY;

    public float speedDefault, speedModifierDistance;

    //public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + manualOffsetY, player.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        target.x = player.transform.position.x + (offsetX * player.GetComponentInChildren<PlayerMovement>().playerFacingDirection);

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
