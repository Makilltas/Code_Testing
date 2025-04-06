using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 40;
    public float LifeTime = 3;
    public Vector2 damageRange = new Vector2(10, 20);

    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, LifeTime);

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            direction = transform.forward;
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        var damage = Random.Range(damageRange.x, damageRange.y);

        print(damage);

        Destroy(gameObject);
    }
}
