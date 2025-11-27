using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public GameObject forby;

    public int damage;

    public bool wasShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = forby.GetComponent<ForbyBehaviour>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasShot == true && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Gate"))
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage);
        }
    }
}
