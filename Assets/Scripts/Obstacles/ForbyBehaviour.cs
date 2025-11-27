using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ForbyBehaviour : MonoBehaviour
{
    public GameObject player;

    public GameObject playerColor;

    public GameObject[] bulletArray;

    public SpriteRenderer[] bulletColors;

    public bool dead;

    public int damage;

    public float range;

    public float shootingCooldown;
    public float shootingCooldownTimer;
    public float shootingSpeed;
    public float bulletRespawnCooldownMultiplier;

    public Vector2 shootingPositionOffset;
    public Vector2 shootingPosition;
    public Vector2 lastPlayerPositionInRange;

    public int maxNumberOfShots = 4;
    public int currentNumberOfShots;

    public float rotationSpeed;
    public int numberOfRotationsPerShot;
    public int rotationState;

    public bool playerInRange;

    public int[] pattern = new int[4];

    public int fireballsDefeated;

    private int blue = 1;
    private int green = 2;
    private int yellow = 3;
    private int red = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletColors = new SpriteRenderer[4];

        shootingPosition = new Vector2(transform.position.x + shootingPositionOffset.x, transform.position.y + shootingPositionOffset.y);
        shootingCooldown = (360 / rotationSpeed) * (1 / 4f);
        shootingCooldownTimer = shootingCooldown;

        for (int i = 0; i < pattern.Length; i++)
        {
            pattern[i] = Random.Range(1, 5);
            if (i != 0)
            {
                if (pattern[i] == pattern[i - 1])
                {
                    pattern[i] = pattern[i - 1] + 1;
                    if (pattern[i] > pattern.Length - 1)
                    {
                        pattern[i] = 1;
                    }
                }
            }
        }

        for (int i = 0; i < bulletArray.Length; i++)
        {
            bulletColors[i] = bulletArray[i].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < bulletArray.Length; i++)
        {
            if (pattern[i] == yellow)
            {
                bulletColors[i].color = Color.yellow;
            }
            else if (pattern[i] == green)
            {
                bulletColors[i].color = Color.green;
            }
            else if (pattern[i] == blue)
            {
                bulletColors[i].color = Color.blue;
            }
            else if (pattern[i] == red)
            {
                bulletColors[i].color = Color.red;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotating
        BulletRotation();


        //Shooting
        ShootingBullets();

        //Reseting Bullets After Shooting
        if (shootingCooldownTimer <= -bulletRespawnCooldownMultiplier * shootingCooldown && rotationState == 4)
        {
            rotationState = 0;
            shootingCooldownTimer = shootingCooldown;
        }

        //Color
        ColorInteraction();

        //Destroying Bullets Upon Death
        if (fireballsDefeated == 4)
        {
            dead = true;
        }

        if (dead == true)
        {
            for (int i = 0; i < bulletArray.Length; i++)
            {
                bulletArray[i].SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage);
        }
    }


    //Shooting Functions
    void BulletRotation()
    {
        if (rotationState == 0)
        {
            fireballsDefeated = 0;
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

    //Color Functions
    void ColorInteraction()
    {
        for (int i = 0; i < bulletArray.Length; i++)
        {
            if (bulletArray[i].GetComponent<EnemyBulletBehaviour>().playerMusicAreaInRange == true)
            {
                if (bulletArray[i].activeSelf == true && bulletArray[i].GetComponent<EnemyBulletBehaviour>().wasShot == true)
                {
                    if ((playerColor.GetComponent<MusicPlay>().color == pattern[i]) && (fireballsDefeated == i))
                    {
                        bulletArray[i].SetActive(false);
                        fireballsDefeated++;
                    }
                }
            }
        }
    }
}
