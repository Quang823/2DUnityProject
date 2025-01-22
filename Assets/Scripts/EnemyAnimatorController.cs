using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    public Animator animator; // Animator của quái
    public Transform player; // Transform của người chơi
    public Transform ememyQua;
    public float detectionRange = 5f; // Phạm vi phát hiện người chơi
    public float attackRange = 1.5f; // Phạm vi tấn công
    public float moveSpeed = 2f; // Tốc độ di chuyển của quái
    private bool isDead = false; // Trạng thái quái đã chết
    private float health = 100f; // Máu của quái


    void Update()
    {
        if (isDead) return;

        // Tính khoảng cách tới người chơi
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRange)
        {
            SetIdle(); // Không phát hiện người chơi
        }
        else if (distanceToPlayer > attackRange)
        {
            SetRunning(); // Phát hiện nhưng chưa tới phạm vi tấn công
            MoveTowardsPlayer(); // Di chuyển về phía người chơi
        }
        else
        {
            SetAttacking(); // Trong phạm vi tấn công
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("isHurt"); // Kích hoạt trạng thái bị thương

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("isDead");

        // Xóa quái sau 2 giây, đảm bảo không bị gọi nhầm
        Destroy(gameObject, 2f);
    }


    private void SetIdle()
    {
        ResetAllTriggers();
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
    }

    private void SetRunning()
    {
        ResetAllTriggers();
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", true);
    }

    private void SetAttacking()
    {
        ResetAllTriggers();
        animator.SetTrigger("isAttacking");
    }

    private void MoveTowardsPlayer()
    {
        // Xác định hướng di chuyển
        Vector3 direction = (player.position - transform.position).normalized;

        // Di chuyển quái
        transform.position += new Vector3(direction.x, 0, 0) * moveSpeed * Time.deltaTime;

        // Lật quái nếu cần thiết
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(5, 5, 5); // Hướng sang phải
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5); // Hướng sang trái
        }
    }



    private void ResetAllTriggers()
    {
        animator.ResetTrigger("isHurt");
        animator.ResetTrigger("isDead");
        animator.ResetTrigger("isAttacking");
    }
}
