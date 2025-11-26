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

    public int maxNumberOfShots = 4;
    public int currentNumberOfShots;

    public float rotationSpeed;
    public int numberOfRotationsPerShot;
    public int rotationState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootingPosition = new Vector2(transform.position.x + shootingPositionOffset.x, transform.position.y + shootingPositionOffset.y);
        shootingCooldownTimer = shootingCooldown;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotating
        BulletRotation();


        //Shooting
        if (currentNumberOfShots < maxNumberOfShots && rotationState == 4 && dead == false)
        {
            //if (Vector2.Distance(player.transform.position, transform.position) <= range)
            //{
                shootingCooldownTimer -= Time.fixedDeltaTime;

                if (shootingCooldownTimer <= 0)
                {
                    shootingCooldownTimer = shootingCooldown;

                    Vector2 shootingDirectionalForce = new Vector2(player.transform.position.x - shootingPosition.x, player.transform.position.y - shootingPosition.y);

                    float shootingMagnitude = Mathf.Sqrt(Mathf.Pow(shootingDirectionalForce.x, 2) + Mathf.Pow(shootingDirectionalForce.y, 2));


                    for (int i = 0; i < bulletArray.Length; i++)
                    {
                        if (bulletArray[i].activeSelf == false)
                        {
                            bulletArray[i].transform.position = shootingPosition;
                            //bulletArray[i].SetActive(true);
                            bulletArray[i].GetComponent<Rigidbody2D>().linearVelocityX = (shootingDirectionalForce.x / shootingMagnitude) * shootingSpeed;
                            bulletArray[i].GetComponent<Rigidbody2D>().linearVelocityY = (shootingDirectionalForce.y / shootingMagnitude) * shootingSpeed;
                            currentNumberOfShots++;
                            return;
                        }
                    }
                }
            //}
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerInteractions>().hp -= damage;
        }
    }

    void BulletRotation()
    {
        if (rotationState == 0)
        {
            bulletArray[0].GetComponent<EnemyBulletBehaviour>().wasShot = false;
            bulletArray[0].SetActive(true);
            bulletArray[0].transform.position = shootingPosition;
            rotationState = 1;
        }
        else if (rotationState == 1)
        {
            bulletArray[0].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 270 && bulletArray[0].transform.rotation.eulerAngles.z > 180)
            {
                bulletArray[1].GetComponent<EnemyBulletBehaviour>().wasShot = false;
                bulletArray[1].SetActive(true);
                bulletArray[1].transform.position = shootingPosition;
                rotationState = 2;
            }
        }
        else if (rotationState == 2)
        {
            bulletArray[0].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[1].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 180 && bulletArray[0].transform.rotation.eulerAngles.z > 90)
            {
                bulletArray[2].GetComponent<EnemyBulletBehaviour>().wasShot = false;
                bulletArray[2].SetActive(true);
                bulletArray[2].transform.position = shootingPosition;
                rotationState = 3;
            }
        }
        else if (rotationState == 3)
        {
            bulletArray[0].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[1].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[2].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            if (bulletArray[0].transform.rotation.eulerAngles.z <= 90 && bulletArray[0].transform.rotation.eulerAngles.z > 0)
            {
                bulletArray[3].GetComponent<EnemyBulletBehaviour>().wasShot = false;
                bulletArray[3].SetActive(true);
                bulletArray[3].transform.position = shootingPosition;
                rotationState = 4;
            }
        }
        else if (rotationState == 4)
        {
            bulletArray[0].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[1].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[2].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
            bulletArray[3].transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
