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
        FindPlayer(); // Tìm player trong Start

        animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (isDead || isAttacking) return;

        // Kiểm tra và tìm lại player nếu cần
        if (player == null)
        {
            FindPlayer();
            if (player == null) return; // Nếu vẫn không tìm thấy player, dừng Update
        }

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

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            player = null;
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
}