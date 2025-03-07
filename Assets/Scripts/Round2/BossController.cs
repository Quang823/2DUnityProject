using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private int health = 200;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float speed = 2f;

    [Header("References")]
    private Animator animator;
    private Transform player;
    private bool isAttacking = false;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            Attack();
        }
        else if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("isRunning", true);
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("IsAttacking");
        Invoke("ResetAttack", 1f); // Đợi 1 giây để kết thúc animation
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 3f);
    }
}
