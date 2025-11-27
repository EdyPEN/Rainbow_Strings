using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ForbyBehaviour : MonoBehaviour
{
    public GameObject player;

    public GameObject[] bulletArray;

    public bool dead;

    public int damage;

    public float range;

    public float shootingCooldown;
    public float shootingCooldownTimer;
    public float shootingSpeed;

    public Vector2 shootingPositionOffset;
    public Vector2 shootingPosition;
    public Vector2 lastPlayerPositionInRange;

    public int maxNumberOfShots = 4;
    public int currentNumberOfShots;

    public float rotationSpeed;
    public int numberOfRotationsPerShot;
    public int rotationState;

    public bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootingPosition = new Vector2(transform.position.x + shootingPositionOffset.x, transform.position.y + shootingPositionOffset.y);
        shootingCooldown = (360 / rotationSpeed) * (1 / 4f);
        shootingCooldownTimer = shootingCooldown;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotating
        BulletRotation();


        //Shooting
        ShootingBullets();

        //Reseting Bullets After Shooting
        if (shootingCooldownTimer <= -shootingCooldown && rotationState == 4)
        {
            rotationState = 0;
            shootingCooldownTimer = shootingCooldown;
        }

        //Destroying Bullets Upon Death
        if (dead == true)
        {
            for (int i = 0; i < bulletArray.Length; i++)
            {
                bulletArray[rotationState].SetActive(false);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage);
        }
    }

    void BulletRotation()
    {
        if (rotationState == 0)
        {
            SpawnRotatingBullet();
        }
        else if (rotationState == 1)
        {
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 270 && bulletArray[0].transform.rotation.eulerAngles.z > 180)
            {
                SpawnRotatingBullet();
            }
        }
        else if (rotationState == 2)
        {
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 180 && bulletArray[0].transform.rotation.eulerAngles.z > 90)
            {
                SpawnRotatingBullet();
            }
        }
        else if (rotationState == 3)
        {
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 90 && bulletArray[0].transform.rotation.eulerAngles.z > 0)
            {
                SpawnRotatingBullet();
            }
        }
        if (rotationState != 0)
        {
            for (int i = 0; i < rotationState; i++)
            {
                if (bulletArray[i].GetComponent<EnemyBulletBehaviour>().wasShot == false)
                {
                    bulletArray[i].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
                }
            }
        }

    }

    void SpawnRotatingBullet()
    {
        bulletArray[rotationState].SetActive(true);
        bulletArray[rotationState].transform.eulerAngles = new Vector3(0, 0, 0);
        bulletArray[rotationState].GetComponent<Rigidbody2D>().linearVelocityX = 0;
        bulletArray[rotationState].GetComponent<Rigidbody2D>().linearVelocityY = 0;
        bulletArray[rotationState].GetComponent<EnemyBulletBehaviour>().wasShot = false;
        bulletArray[rotationState].transform.position = shootingPosition;
        rotationState++;
    }

    void ShootingBullets()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= range)
        {
            lastPlayerPositionInRange = player.transform.position;
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
        if (rotationState == 4 && (playerInRange == true || bulletArray[0].GetComponent<EnemyBulletBehaviour>().wasShot == true))
        {
            shootingCooldownTimer -= Time.fixedDeltaTime;

            if (shootingCooldownTimer <= 0)
            {

                Vector2 shootingDirectionalForce = new Vector2(lastPlayerPositionInRange.x - shootingPosition.x, lastPlayerPositionInRange.y - shootingPosition.y);

                float shootingMagnitude = Mathf.Sqrt(Mathf.Pow(shootingDirectionalForce.x, 2) + Mathf.Pow(shootingDirectionalForce.y, 2));

                for (int i = 0; i < bulletArray.Length; i++)
                {
                    if (bulletArray[i].GetComponent<EnemyBulletBehaviour>().wasShot == false)
                    {
                        bulletArray[i].transform.position = shootingPosition;
                        bulletArray[i].GetComponent<Rigidbody2D>().linearVelocityX = (shootingDirectionalForce.x / shootingMagnitude) * shootingSpeed;
                        bulletArray[i].GetComponent<Rigidbody2D>().linearVelocityY = (shootingDirectionalForce.y / shootingMagnitude) * shootingSpeed;
                        bulletArray[i].GetComponent<EnemyBulletBehaviour>().wasShot = true;
                        currentNumberOfShots++;
                        shootingCooldownTimer = shootingCooldown;
                        return;
                    }
                }
            }
        }
    }
}
