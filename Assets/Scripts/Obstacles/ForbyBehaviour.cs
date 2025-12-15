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
    Rigidbody2D rigidBody;

    public GameObject player;
    public GameObject playerColor;

    public GameObject[] note;
    public GameObject[] bullet;

    public SpriteRenderer[] bulletSpriteRenderer;

    public Vector2 bulletSpawningPosition;
    public Vector2 lastPlayerPositionInRange;

    public int damage;

    public float range;
    public bool playerInRange;

    public float shootingSpeed;
    public float shootingCooldownTime;
    public float shootingCooldownTimer;
    public float bulletRespawnCooldownMultiplier;

    public int maxNumberOfShots;
    public int fireballsDefeated;
    public int currentNumberOfShots;

    public int rotationState;
    public float rotationSpeed;
    public int numberOfRotationsPerShot;

    public bool dead;
    public float deathJumpTime;
    public float deathJumpTimer;
    public float deathJumpHeight;

    public MusicPlay.MusicKey[] pattern = new MusicPlay.MusicKey[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        deathJumpTimer = deathJumpTime;

        bulletSpriteRenderer = new SpriteRenderer[4];

        bulletSpawningPosition += (Vector2) transform.position;
        shootingCooldownTime = (360 / rotationSpeed) * (1 / 4f);
        shootingCooldownTimer = shootingCooldownTime;

        PatternRandomizer(pattern);

        BulletColors();
    }

    void BulletColors()
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            bulletSpriteRenderer[i] = bullet[i].GetComponent<SpriteRenderer>();
        }

        for (int i = 0; i < bullet.Length; i++)
        {
            if (pattern[i] == MusicPlay.MusicKey.Yellow)
            {
                bulletSpriteRenderer[i].color = Color.yellow;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Green)
            {
                bulletSpriteRenderer[i].color = Color.green;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Blue)
            {
                bulletSpriteRenderer[i].color = Color.deepSkyBlue;
            }
            else if (pattern[i] == MusicPlay.MusicKey.Red)
            {
                bulletSpriteRenderer[i].color = Color.red;
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
    void Update()
    {
        BulletRotation();

        CheckLastPlayerPosition();

        ShootingBullets();

        ResettingBulletsAfterShooting();

        ColorInteraction();

        DestroyingBulletsUponDeath();

        NoteColors();
    }

    void BulletRotation()
    {
        if (rotationState == 0)
        {
            fireballsDefeated = 0;
            SpawnRotatingBullet();
        }
        else if (rotationState == 1)
        {
            if (bullet[0].transform.rotation.eulerAngles.z <= 270 && bullet[0].transform.rotation.eulerAngles.z > 180)
            {
                SpawnRotatingBullet();
            }
        }
        else if (rotationState == 2)
        {
            if (bullet[0].transform.rotation.eulerAngles.z <= 180 && bullet[0].transform.rotation.eulerAngles.z > 90)
            {
                SpawnRotatingBullet();
            }
        }
        else if (rotationState == 3)
        {
            if (bullet[0].transform.rotation.eulerAngles.z <= 90 && bullet[0].transform.rotation.eulerAngles.z > 0)
            {
                SpawnRotatingBullet();
            }
        }
        if (rotationState != 0)
        {
            for (int i = 0; i < rotationState; i++)
            {
                if (bullet[i].GetComponent<EnemyBulletBehaviour>().wasShot == false)
                {
                    bullet[i].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
                }
            }
        }

    }

    void SpawnRotatingBullet()
    {
        bullet[rotationState].SetActive(true);
        bullet[rotationState].transform.eulerAngles = Vector3.zero;
        bullet[rotationState].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        bullet[rotationState].GetComponent<EnemyBulletBehaviour>().wasShot = false;
        bullet[rotationState].transform.position = bulletSpawningPosition;
        note[rotationState].SetActive(true);
        rotationState++;
    }

    void CheckLastPlayerPosition()
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
    }

    void ShootingBullets()
    {
        if (rotationState == 4 && (playerInRange == true || bullet[0].GetComponent<EnemyBulletBehaviour>().wasShot == true))
        {
            shootingCooldownTimer -= Time.deltaTime;

            if (shootingCooldownTimer <= 0)
            {
                for (int i = 0; i < bullet.Length; i++)
                {
                    Vector2 shootingDirectionalForce = new Vector2(lastPlayerPositionInRange.x - bullet[i].transform.position.x, lastPlayerPositionInRange.y - bullet[i].transform.position.y);

                    float shootingMagnitude = Mathf.Sqrt(Mathf.Pow(shootingDirectionalForce.x, 2) + Mathf.Pow(shootingDirectionalForce.y, 2));

                    if (bullet[i].GetComponent<EnemyBulletBehaviour>().wasShot == false)
                    {
                        bullet[i].GetComponent<Rigidbody2D>().linearVelocityX = (shootingDirectionalForce.x / shootingMagnitude) * shootingSpeed;
                        bullet[i].GetComponent<Rigidbody2D>().linearVelocityY = (shootingDirectionalForce.y / shootingMagnitude) * shootingSpeed;
                        bullet[i].GetComponent<EnemyBulletBehaviour>().wasShot = true;
                        currentNumberOfShots++;
                        shootingCooldownTimer = shootingCooldownTime;
                        return;
                    }
                }
            }
        }
    }

    void ResettingBulletsAfterShooting()
    {
        if (shootingCooldownTimer <= -bulletRespawnCooldownMultiplier * shootingCooldownTime && rotationState == 4)
        {
            rotationState = 0;
            shootingCooldownTimer = shootingCooldownTime;
        }
    }

    void ColorInteraction()
    {
        for (int i = 0; i < bullet.Length; i++)
        {
            if (bullet[i].GetComponent<EnemyBulletBehaviour>().playerMusicAreaInRange == true)
            {
                if (bullet[i].activeSelf == true && bullet[i].GetComponent<EnemyBulletBehaviour>().wasShot == true)
                {
                    for (int j = 0; j < note.Length; j++)
                    {
                        note[j].SetActive(true);
                    }
                    if ((playerColor.GetComponent<MusicPlay>().key == pattern[i]))
                    {
                        bullet[i].SetActive(false);
                        if (i == fireballsDefeated)
                        {
                            fireballsDefeated++;
                        }
                    }
                    else if (playerColor.GetComponent<MusicPlay>().key != MusicPlay.MusicKey.Idle)
                    {
                        fireballsDefeated = 0;
                    }
                }
            }
        }
    }

    void DestroyingBulletsUponDeath()
    {
        if (fireballsDefeated == 4)
        {
            dead = true;

            rigidBody.linearVelocity = new Vector2(0, deathJumpHeight);

            rigidBody.gravityScale = 3;

            GetComponent<Collider2D>().enabled = false;

            for (int i = 0; i < bullet.Length; i++)
            {
                bullet[i].SetActive(false);
            }
        }
        if (dead == true)
        {
            deathJumpTimer -= Time.deltaTime;
            if (deathJumpTimer < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void NoteColors()
    {
        if (fireballsDefeated == 0)
        {
            for (int i = 0; i < note.Length; i++)
            {
                note[i].GetComponent<Note>().key = MusicPlay.MusicKey.Idle;
            }
        }

        for (int i = 0; i < note.Length; i++)
        {
            if (i + 1 == fireballsDefeated)
            {
                note[i].GetComponent<Note>().key = pattern[i];
            }
        }

        if (playerInRange)
        {
            for (int i = 0; i < note.Length; i++)
            {
                note[i].GetComponent<Note>().hideNotes = false;
            }
        }
        else
        {
            for (int i = 0; i < note.Length; i++)
            {
                note[i].GetComponent<Note>().hideNotes = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().TakeDamage(damage, true, collision);
        }
    }

}
