using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;
    public Transform player;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = true;
    private enum State { Idle, Walk, Attack, Hurt, Dead }
    private State currentState = State.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            ChangeState(State.Attack);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChangeState(State.Walk);
            MoveTowardsPlayer();
        }
        else
        {
            ChangeState(State.Idle);
        }
    }

    void MoveTowardsPlayer()
    {
        if (currentState != State.Walk) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);  // Sửa lại từ linearVelocity thành velocity

        if (direction.x > 0 && !isFacingRight)
            Flip();
        else if (direction.x < 0 && isFacingRight)
            Flip();
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        // Kiểm tra trạng thái hiện tại và set các Bool trong Animator
        switch (newState)
        {
            case State.Idle:
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                break;
            case State.Walk:
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                break;
            case State.Attack:
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                break;
            case State.Hurt:
                anim.SetTrigger("isHurt");
                break;
            case State.Dead:
                anim.SetTrigger("isDead");
                break;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void TakeDamage()
    {
        ChangeState(State.Hurt);
        // Giảm máu ở đây nếu có hệ thống máu
        Invoke("ReturnToIdle", 1f);
    }

    void ReturnToIdle()
    {
        if (currentState != State.Dead)
            ChangeState(State.Idle);
    }

    public void Die()
    {
        ChangeState(State.Dead);
        rb.linearVelocity = Vector2.zero;  // Dừng mọi chuyển động
        this.enabled = false; // Vô hiệu hóa script khi boss chết
    }
}
