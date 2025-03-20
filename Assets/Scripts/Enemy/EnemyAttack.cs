using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 10; // Sát thương của quái
    [SerializeField] private float attackCooldown = 1.5f; // Thời gian giữa các lần tấn công
    private float lastAttackTime;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Debug.Log("Quái gây sát thương! Máu còn lại: " + playerStats.GetCurrentHealth());
                lastAttackTime = Time.time;
            }
        }
    }
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
            Debug.Log("Boss tấn công! Máu còn lại của Player: " + playerStats.GetCurrentHealth());
            lastAttackTime = Time.time;
        }
    }
}


}
