using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    private bool isAttacking;
    private float attackDuration = 10f;
    private float attackTimer;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float boxcastYOffset = -0.3f;


    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
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
            attackTimer = attackDuration;
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }

            if (enemyPatrol != null)
            {
                enemyPatrol.PausePatrol();
            }
        }
        else
        {
            PlayerStats player = FindFirstObjectByType<PlayerStats>();
            if (player != null && player.GetCurrentHealth() > 0)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    if (enemyPatrol != null)
                    {
                        enemyPatrol.enabled = true;
                    }
                }
            }
        }
    }


    private void RangedAttack()
    {
        cooldownTimer = 0;

        int fireballIndex = FindFireball();
        if (fireballIndex == -1)
        {
            GameObject newFireball = Instantiate(fireballs[0], firepoint.position, Quaternion.identity);
            newFireball.SetActive(false);
            System.Array.Resize(ref fireballs, fireballs.Length + 1);
            fireballs[fireballs.Length - 1] = newFireball;
            fireballIndex = fireballs.Length - 1;
        }

        GameObject fireball = fireballs[fireballIndex];
        fireball.transform.position = firepoint.position;

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;

        fireball.GetComponent<EnemyProjectile>().ActivateProjectile(direction);
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return -1;
    }

    private bool PlayerInSight()
    {
        float adjustedHeight = boxCollider.bounds.size.y * 0.5f;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, adjustedHeight);
        Vector2 boxCenter = (Vector2)boxCollider.bounds.center +
                            (Vector2.right * transform.localScale.x * range * colliderDistance) +
                            new Vector2(0, boxcastYOffset);

        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.right * transform.localScale.x, 0, playerLayer);

        if (hit.collider != null)
        {
            PlayerStats player = hit.collider.GetComponent<PlayerStats>();
            if (player != null && player.GetCurrentHealth() > 0)
            {
                return true;
            }
        }
        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center +
                            transform.right * range * transform.localScale.x * colliderDistance +
                            new Vector3(0, boxcastYOffset, 0),
                            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * 0.5f, boxCollider.bounds.size.z));
    }


    private void DamagePlayer()
    {
        float adjustedHeight = boxCollider.bounds.size.y * 0.5f;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, adjustedHeight);
        Vector3 boxCenter = (Vector3)boxCollider.bounds.center +
                    (Vector3)(Vector2.right * transform.localScale.x * range * colliderDistance) +
                    new Vector3(0, boxcastYOffset, 0);

        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.zero, 0, playerLayer);
        if (hit.collider != null)
        {
            PlayerStats player = hit.collider.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }


}