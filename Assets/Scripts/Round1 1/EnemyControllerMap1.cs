using UnityEngine;

public class EnemyControllerMap1 : MonoBehaviour
{
    private Animator anim;
    private bool isAttacking = false;
    private bool canAttack = true; // Biến kiểm soát cooldown
    private Collider2D playerCollider;

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
            anim.SetBool("isAttack", true);
            playerCollider = collision;

            isAttacking = true;
            canAttack = false;

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    public void DealDamage()
    {

        if (playerCollider != null)
        {
            PlayerStats playerStats = playerCollider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                Debug.Log("Gọi TakeDamage() trên PlayerStats!");
                playerStats.TakeDamage(10);
            }
            else
            {
                Debug.Log("Không tìm thấy PlayerStats trên playerCollider!");
            }
        }
        else
        {
            Debug.Log("playerCollider == null, không có va chạm!");
        }
    }

    private void ResetAttack()
    {
        isAttacking = false; 
        canAttack = true;
        anim.SetBool("isAttack", false);
        if (walkScript != null)
        {
            walkScript.StartMoving(); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isAttack", false);
        }
    }
}
