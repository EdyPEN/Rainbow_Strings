using UnityEngine;

public class RippleBehaviour : MonoBehaviour
{
    public GameObject player;

    SpriteRenderer spriteRenderer;

    public int direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectingPlayerDirection();

        ChangeDirection();
    }

    void DetectingPlayerDirection()
    {
        if (player.transform.position.x < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    void ChangeDirection()
    {
        if (direction == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction == -1)
        {
            spriteRenderer.flipX = true;
        }
    }
}
