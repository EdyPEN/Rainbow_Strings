using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ForbyBehaviour : MonoBehaviour
{
    public GameObject player;

    public List<GameObject> bulletPool;

    public bool dead;

    public int damage;

    public float range;

    public float shootingCooldown;
    public float shootingCooldownTimer;
    public float shootingSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootingCooldownTimer = shootingCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= range)
        {
                shootingCooldownTimer -= Time.fixedDeltaTime;

            if (shootingCooldownTimer <= 0 && dead == false)
            {
                shootingCooldownTimer = shootingCooldown;

                Vector2 shootingDirectionalForce = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

                float shootingMagnitude = Mathf.Sqrt(Mathf.Pow(shootingDirectionalForce.x, 2) + Mathf.Pow(shootingDirectionalForce.y, 2));


                for (int i = 0; i < bulletPool.Count; i++)
                {
                    if (bulletPool[i].activeSelf == false)
                    {
                        bulletPool[i].transform.position = transform.position;
                        bulletPool[i].SetActive(true);
                        bulletPool[i].GetComponent<Rigidbody2D>().linearVelocityX = (shootingDirectionalForce.x / shootingMagnitude) * shootingSpeed;
                        bulletPool[i].GetComponent<Rigidbody2D>().linearVelocityY = (shootingDirectionalForce.y / shootingMagnitude) * shootingSpeed;
                        return;
                    }
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().hp -= damage;
        }
    }
}
