using System.Collections;
using UnityEngine;

public class TurretEmeny : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2.0f;
    public float bulletSpeed = 10f;
    public float detectionRange = 10f;


    public int damage = 1;

    public int eHealth = 10;



    private Transform player;
    private float fireCooldown;


    void Start()
    {


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;


        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {

        fireCooldown -= Time.deltaTime;
        if (fireCooldown > 0) return;


        fireCooldown = fireRate;


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);


        Vector2 direction = (player.position - firePoint.position).normalized;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);


        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction * bulletSpeed;
        }

        
    }


    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    
}