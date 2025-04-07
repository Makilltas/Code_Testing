using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float bulletCooldown = 2f;
    public float bulletSpeed = 20f;

    public GameObject laserObject;
    public float laserCooldown = 10f;
    public float laserChargeTime = 2f;
    public float laserDuration = 3f;

    private float bulletTimer;
    private float laserTimer;
    private bool isFiringLaser = false;

    void Update()
    {
        if (player == null) return;

        bulletTimer -= Time.deltaTime;
        laserTimer -= Time.deltaTime;

        if (bulletTimer <= 0f && !isFiringLaser)
        {
            FireBullet();
            bulletTimer = bulletCooldown;
        }

        if (laserTimer <= 0f && !isFiringLaser)
        {
            StartCoroutine(FireLaserRoutine());
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

    IEnumerator FireLaserRoutine()
    {
        isFiringLaser = true;

        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        float t = 0f;
        Quaternion startRot = firePoint.rotation;

        while (t < laserChargeTime)
        {
            firePoint.rotation = Quaternion.Slerp(startRot, lookRotation, t / laserChargeTime);
            t += Time.deltaTime;
            yield return null;
        }

        firePoint.rotation = lookRotation;

        float distance = Vector3.Distance(firePoint.position, player.position);

        laserObject.SetActive(true);

       
        laserObject.transform.position = firePoint.position;
        laserObject.transform.rotation = Quaternion.LookRotation(directionToPlayer);

        
        laserObject.transform.localScale = new Vector3(5f, 5f, distance);

        yield return new WaitForSeconds(laserDuration);

        laserObject.SetActive(false);
        isFiringLaser = false;
    }
}
