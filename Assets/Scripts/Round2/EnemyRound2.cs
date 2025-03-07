using UnityEngine;

public class EnemyRound2 : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;
    [SerializeField] private int health = 3;
    [SerializeField] private float attackRange = 1.5f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private bool isAttacking = false;
    private bool isDead = false;

    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Giữ quái là Kinematic để nó không bị lực đẩy
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        startPosition = transform.position;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (isDead || isAttacking) return;

        float leftBoundary = startPosition.x - distance;
        float rightBoundary = startPosition.x + distance;

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

        // Kiểm tra khoảng cách với Player để tấn công
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        animator.SetTrigger("IsAttacking");

        // Dừng di chuyển khi tấn công
        speed = 0;
        rb.linearVelocity = Vector2.zero;

        Invoke("ResetAttack", 1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        speed = 2f;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        health--;
        if (health <= 0)
        {
            Die();
        }
    }

   void Die()
{
    if (isDead) return;
    isDead = true;
    
    Debug.Log("EnemyRound2 Die() called");
    
    animator.SetBool("dead", true);
    GetComponent<Collider2D>().enabled = false;
    Destroy(gameObject, 2f);
}

}
