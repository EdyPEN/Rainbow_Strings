using UnityEngine;

public class KeyMovement : MonoBehaviour
{
    public Transform Target;
    public GameObject Player;

    public float speed;
    public float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (Player.GetComponent<PlayerInteractions>().keyCollected == true)
        {
            float currentDistance = Vector3.Distance(transform.transform.position, Target.transform.position);

            Vector2 currentPosition2D = new Vector3(transform.position.x, transform.position.y);
            Vector2 targetPosition2D = new Vector3(Target.position.x, Target.position.y);

            if (currentDistance > distance)
                transform.position = Vector2.MoveTowards(currentPosition2D, targetPosition2D, speed * Time.fixedDeltaTime);
        }

        if (Player.GetComponent<PlayerInteractions>().gateOpened == true)
        {
            gameObject.SetActive(false);
        }
    }
}
