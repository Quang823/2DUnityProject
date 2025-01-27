using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Attack Parameter")]
    [SerializeField] private float attackCooldown; 
    [SerializeField] private float range; 
    [SerializeField] private int damage; 

    [Header("Attack Parameter")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer; 
    private float cooldownTimer = Mathf.Infinity; 

    private Animator anim; 
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        Vector2 boxCenter = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.zero, 0, playerLayer);
        return hit.collider != null;
    }

    private void DamagePalyer()
    {       
        Vector2 boxCenter = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);     
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.zero, 0, playerLayer);
        if (hit.collider != null)
        {        
            PlayerStats player = hit.collider.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
               // Debug.Log($"Player bị tấn công: {damage} sát thương");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
