using System.Collections;
using UnityEngine;

public class BossAimShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;
    public float bulletSpeed = 5f;
    public float fireRate = 1.5f;
    public float attackRange = 5f;
    public float detectionRange = 10f; // Khoảng cách phát hiện Player
    public Animator animator;

    private float nextFireTime;
    private bool isDead = false;

    void Update()
    {
        if (player != null && !isDead)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            animator.SetFloat("distance", distance);


            FlipBoss();

            if (distance <= detectionRange)
            {
                animator.SetBool("Fly", true);
            }
            else
            {
                animator.SetBool("Fly", false);
            }

            if (distance <= attackRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0, 0, angle + 180);

                if (Time.time > nextFireTime)
                {
                    Shoot(direction);
                    nextFireTime = Time.time + fireRate;
                    animator.SetTrigger("Attack");
                }
            }
        }
    }
    void FlipBoss()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        float direction = player.position.x - transform.position.x;

        if (direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
    }



    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }

}
