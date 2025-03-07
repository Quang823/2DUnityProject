using UnityEngine;

public class EnemyController1 : MonoBehaviour
{
    private Animator anim;
    private bool isAttacking = false;
    private bool canAttack = true; // Biến kiểm soát cooldown

    [SerializeField] private Walk walkScript;

    [SerializeField] private float attackCooldown = 0.45f; // Thời gian delay giữa các lần tấn công

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing from the enemy object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttacking && canAttack)
        {

            anim.SetBool("isAttack", true); // Chạy animation Attack một lần
            if (walkScript != null)
            {
                walkScript.StopMoving(); // Dừng di chuyển khi tấn công
            }

            isAttacking = true;
            canAttack = false; // Tạm thời khóa tấn công

            Invoke(nameof(ResetAttack), attackCooldown); // Reset sau cooldown
        }
    }

    private void ResetAttack()
    {
        isAttacking = false; // Cho phép Attack lại
        canAttack = true; // Mở lại khả năng tấn công
        anim.SetBool("isAttack", false);
        if (walkScript != null)
        {
            walkScript.StartMoving(); // Tiếp tục di chuyển sau khi tấn công
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player left the collision area
        if (collision.CompareTag("Player"))
        {
            // Revert to the previous animation (e.g., Idle)
            anim.SetBool("isAttack", false);
        }
    }

}
