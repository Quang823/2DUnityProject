using UnityEngine;

public class EnemyAttackRound2 : MonoBehaviour
{
    [Header("Attack Parameter")]
    [SerializeField] private float attackCooldown; 
    [SerializeField] private float range; 
    [SerializeField] private int damage; 

    [Header("Collider Parameter")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player ")]
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
                anim.SetTrigger("IsAttacking");
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

public void DamagePalyer()
{
    Vector2 boxCenter = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
    Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);
    RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.zero, 0, playerLayer);

    if (hit.collider != null)
    {
        Debug.Log("Quái đánh trúng: " + hit.collider.name);

        // Tìm Player thông qua transform cha nếu trúng phải object con
        PlayerStats player = hit.collider.GetComponent<PlayerStats>();
        if (player == null)
        {
            player = hit.collider.GetComponentInParent<PlayerStats>(); // Lấy từ cha
        }

        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Player mất máu: " + damage);
        }
        else
        {
            Debug.Log("Không tìm thấy PlayerStats trên " + hit.collider.name);
        }
    }
    else
    {
        // Debug.Log("Không trúng ai cả.");
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
