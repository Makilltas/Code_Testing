using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public AudioClip chargeSound;
    public AudioClip fireSound;
    public AudioSource audioSource;

    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public GameObject laserPrefab;
    public Transform laserContainer;

    public float rotationSpeed = 10f;

    public float bulletCooldown = 2f;
    public float bulletSpeed = 25f;

    public float laserCooldown = 10f;
    public float laserChargeTime = 2f;
    public float laserDuration = 3f;

    public float laserLength = 50f;
    public float laserWidth = 5f;
    public float laserHeight = 50f;

    private float bulletTimer;
    private float laserTimer;
    private bool isFiringLaser = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
        }

        bulletTimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;

        if (bulletTimer <= 0f && !isFiringLaser)
        {
            FireBullet();
            bulletTimer = bulletCooldown;
        }

        if (laserTimer <= 0f && !isFiringLaser)
        {
            StartCoroutine(FireLasers());
            laserTimer = laserCooldown;
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (player.position - firePoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }

    IEnumerator FireLasers()
    {
        isFiringLaser = true;

        if (chargeSound && audioSource)
            audioSource.PlayOneShot(chargeSound);

        yield return new WaitForSeconds(laserChargeTime);

        if (fireSound && audioSource)
            audioSource.PlayOneShot(fireSound);

        CreateLaser(firePoint.forward, "FrontLaser");
        CreateLaser(-firePoint.forward, "BackLaser");
        CreateLaser(firePoint.right, "RightLaser");
        CreateLaser(-firePoint.right, "LeftLaser");

        yield return new WaitForSeconds(laserDuration);

        foreach (Transform child in laserContainer)
        {
            Destroy(child.gameObject);
        }

        isFiringLaser = false;
    }

    void CreateLaser(Vector3 direction, string name)
    {
        GameObject laser = Instantiate(laserPrefab, laserContainer);
        laser.name = name;

        laser.transform.rotation = Quaternion.LookRotation(direction);
        laser.transform.localScale = new Vector3(laserWidth, laserWidth, laserLength);
        laser.transform.position = firePoint.position + direction.normalized * (laserLength / 2f);
    }
}
