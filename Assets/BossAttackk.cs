using UnityEngine;

public class BossAttackk : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 20; // Sát thương của boss
    [SerializeField] private float attackCooldown = 2f; // Thời gian giữa các lần tấn công
    [SerializeField] private float attackRange = 2f; // Tầm đánh

    private Animator animator;
    private Transform player;
    private float lastAttackTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        animator.SetTrigger("IsAttacking");
        
        // Gọi hàm gây sát thương sau khi animation bắt đầu
        Invoke("DealDamage", 0.5f); // Delay để phù hợp với animation
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerStats playerStats = hit.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                    Debug.Log("Boss tấn công! Máu của Player: " + playerStats.GetCurrentHealth());
                }
            }
        }
    }

    // Hiển thị vòng tròn tầm đánh trong Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
