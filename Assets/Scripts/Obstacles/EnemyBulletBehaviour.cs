using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public GameObject forby;

    public int damage;

    public bool wasShot;

    public bool playerMusicAreaInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = forby.GetComponent<ForbyBehaviour>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (wasShot == true && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Gate"))
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage, true, collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MusicRange"))
        {
            playerMusicAreaInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MusicRange"))
        {
            playerMusicAreaInRange = false;
        }
    }
}
