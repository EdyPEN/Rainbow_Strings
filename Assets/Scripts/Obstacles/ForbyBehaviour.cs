using JetBrains.Annotations;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static MusicPlay;

public class ForbyBehaviour : MonoBehaviour
{
    public GameObject player;

    public GameObject playerColor;

    public GameObject[] bulletArray;

    public GameObject[] noteArray;

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

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[4];

    public int fireballsDefeated;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < noteArray.Length; i++)
        {
            noteArray[i].SetActive(false);
        }

        bulletColors = new SpriteRenderer[4];

        shootingPosition = new Vector2(transform.position.x + shootingPositionOffset.x, transform.position.y + shootingPositionOffset.y);
        shootingCooldown = (360 / rotationSpeed) * (1 / 4f);
        shootingCooldownTimer = shootingCooldown;

        PatternRandomizer(pattern);
        BulletColors();
    }

    void BulletColors()
    {
        for (int i = 0; i < bulletArray.Length; i++)
        {
            bulletColors[i] = bulletArray[i].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < bulletArray.Length; i++)
        {
            if (pattern[i] == MusicPlay.MusicKey.Yellow)
            {
                bulletColors[i].color = Color.yellow;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Green)
            {
                bulletColors[i].color = Color.green;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Blue)
            {
                bulletColors[i].color = Color.deepSkyBlue;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Red)
            {
                bulletColors[i].color = Color.red;
            }
        }
    }
    void PatternRandomizer(MusicPlay.MusicKey[] pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            int[] keys = (int[])Enum.GetValues(typeof(MusicPlay.MusicKey));
            int minKey = Mathf.Min(keys) + 1;
            int maxKey = Mathf.Max(keys);

            int newKey = UnityEngine.Random.Range(minKey, maxKey + 1);

            if (i != 0)
            {
                if (newKey == (int)pattern[i - 1])
                {
                    ++newKey;
                    if (newKey > maxKey)
                        newKey = minKey;
                }
            }
            pattern[i] = (MusicPlay.MusicKey)newKey;
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

        // Note Resetting
        if (fireballsDefeated == 0)
        {
            for (int i = 0; i < noteArray.Length; i++)
            {
                noteArray[i].GetComponent<Note>().color = idle;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
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
                    if ((playerColor.GetComponent<MusicPlay>().key == pattern[i]))
                    {
                        bulletArray[i].SetActive(true);
                        noteArray[i].GetComponent<Note>().color = playerColor.GetComponent<MusicPlay>().color;
                        fireballsDefeated++;
                    }
                    else if (playerColor.GetComponent<MusicPlay>().color != idle)
                    {
                        fireballsDefeated = 0;
                    }
                }
            }
        }
    }
}
