using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 100f;
    private float currentHealth; 

    public float walkSpeed = 2f; 
    public float runSpeed = 4f;   
    public float attackRange = 1.5f; 
    public float detectionRange = 5f; 

    [Header("References")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    public Transform player;     
    public Animator anim;       

    private Vector3 patrolTarget; 
    private bool movingLeft = true;
    private bool isPlayerDetected = false; 

    private void Start()
    {
        currentHealth = maxHealth;
        patrolTarget = leftPoint.position; 
        anim.SetBool("isWalking", true);   
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die(); 
            return;
        }

        if (isPlayerDetected)
        {
            ChasePlayer(); 
        }
        else
        {
            Patrol(); 
        }

        DetectPlayer(); 
    }

    private void Patrol()
    {
      
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, walkSpeed * Time.deltaTime);

       
        if (Vector2.Distance(transform.position, patrolTarget) < 0.1f)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);

           
            if (patrolTarget == leftPoint.position)
            {
                patrolTarget = rightPoint.position;
                movingLeft = false;
            }
            else
            {
                patrolTarget = leftPoint.position;
                movingLeft = true;
            }

           
            Flip();
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isIdle", false);
        }
    }

    private void DetectPlayer()
    {
      
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isPlayerDetected = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isFlying", true); 
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
           
            anim.SetBool("isRunning", true);
            anim.SetBool("isFlying", false);
            transform.position = Vector2.MoveTowards(transform.position, player.position, runSpeed * Time.deltaTime);
        }
        else
        {
           
            anim.SetBool("isRunning", false);
            anim.SetBool("isFlying", false);
            anim.SetTrigger("attack");
        }

      
        if ((player.position.x < transform.position.x && transform.localScale.x > 0) ||
     (player.position.x > transform.position.x && transform.localScale.x < 0))
        {
            Flip();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("dead"); 
        this.enabled = false; 
        Destroy(gameObject, 2f); 
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        if ((patrolTarget == leftPoint.position && localScale.x > 0) ||
            (patrolTarget == rightPoint.position && localScale.x < 0))
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }


    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
