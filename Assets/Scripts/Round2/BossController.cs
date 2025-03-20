using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private int health = 200;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float patrolDistance = 5f; // Khoảng cách tuần tra

    [Header("References")]
    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;
    
    private Vector3 startPosition;
    private bool movingRight = true;
    private bool isAttacking = false;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator.SetBool("isRunning", true);
    }

    private void Update()
    {
        if (isDead || isAttacking) return;

        float leftBoundary = startPosition.x - patrolDistance;
        float rightBoundary = startPosition.x + patrolDistance;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu Player trong tầm đánh, tấn công
        if (distanceToPlayer <= attackRange)
        {
            Attack();
            return;
        }

        // Tuần tra qua lại trong giới hạn
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        animator.SetTrigger("IsAttacking");

        // Dừng di chuyển khi tấn công
        speed = 0;
        rb.linearVelocity = Vector2.zero;

        Invoke("ResetAttack", 1f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
        speed = 2f;
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
        if (isDead) return;
        isDead = true;

        Debug.Log("Boss has died!");

        animator.SetBool("dead", true);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 3f);
    }
}
