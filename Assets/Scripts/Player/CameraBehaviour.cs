using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;

    public float speed = 2.0f;

    //private Vector2 target;
    //private Vector2 target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPosition2D = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 targetPosition2D = new Vector3(target.position.x, target.position.y, -10);

        transform.position = Vector3.MoveTowards(currentPosition2D, targetPosition2D, speed * Time.fixedDeltaTime);
    }
}
