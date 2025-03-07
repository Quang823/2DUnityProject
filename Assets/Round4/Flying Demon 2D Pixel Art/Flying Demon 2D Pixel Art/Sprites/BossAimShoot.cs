using UnityEngine;

public class BossAimShoot : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab viên ??n
    public Transform firePoint; // V? trí b?n ??n
    public Transform player; // Player ?? nh?m t?i
    public float bulletSpeed = 5f;
    public float fireRate = 1.5f;
    public float attackRange = 5f; // Ph?m vi b?n
    public Animator animator; // Animator ?? ?i?u khi?n tr?ng thái

    private float nextFireTime;

  

    void Update()
    {
        if (player != null)
        {
            // Tính kho?ng cách t?i Player
            float distance = Vector2.Distance(transform.position, player.position);
            animator.SetFloat("distance", distance); // G?i giá tr? distance vào Animator

            // N?u Player trong ph?m vi t?n công
            if (distance <= attackRange)
            {
                // Tính h??ng t? Boss ??n Player
                Vector2 direction = (player.position - transform.position).normalized;

                // ??o h??ng Boss n?u c?n
                float scaleX = Mathf.Abs(transform.localScale.x);
                transform.localScale = new Vector3(player.position.x < transform.position.x ? -scaleX : scaleX, transform.localScale.y, transform.localScale.z);

                // Quay h??ng súng v? phía player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(0, 0, angle + 180);

                // Ki?m tra th?i gian b?n
                if (Time.time > nextFireTime)
                {
                    Shoot(direction);
                    nextFireTime = Time.time + fireRate;

                    // Kích ho?t animation b?n
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }

    public void TakeDamage(float damage)
    {
        animator.SetTrigger("Hurt"); // Kích ho?t animation b? th??ng
        Debug.Log("Boss b? t?n công! Gây sát th??ng: " + damage);
    }
}
