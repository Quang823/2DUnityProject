using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float meleeattackCooldown;
    [SerializeField] private float farattackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackOffset = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int meleeAttackDamage;

    [Header("Far Attack Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] farattack;
    [SerializeField] private float manaCost;

    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private Collider2D playerCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J) && playerMovement.canAttack() && cooldownTimer >= meleeattackCooldown)
        {
            MeleeAttack();
        }

        if (Input.GetKeyDown(KeyCode.K) && playerMovement.canAttack() && cooldownTimer >= farattackCooldown)
        {
            if (playerStats.CanUseSkill(manaCost))
            {
                FireAttack();
                playerStats.UseSkill(manaCost);
            }
        }
    }


    private void MeleeAttack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
    }
    public void DealMeleeDamage()
    {
        if (cooldownTimer < meleeattackCooldown) return; // Đảm bảo không đánh quá nhanh

        Vector2 playerSize = playerCollider.bounds.size;
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(transform.localScale.x * (playerSize.x / 2 + meleeAttackOffset), 0);
        Vector2 attackSize = new Vector2(playerSize.x * 1.5f, playerSize.y * 0.7f);

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(meleeAttackDamage);
        }

        cooldownTimer = 0; 
    }


    private void FireAttack()
    {
        anim.SetTrigger("fireattack");
        cooldownTimer = 0;

        if (farattack.Length > 0 && farattack[currentAttackIndex] != null)
        {
            GameObject projectile = farattack[currentAttackIndex];
            projectile.transform.position = firePoint.position;
            projectile.GetComponent<ProjecTile>().SetDirection(Mathf.Sign(transform.localScale.x));
            projectile.SetActive(true);
            currentAttackIndex = (currentAttackIndex + 1) % farattack.Length;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (GetComponent<Collider2D>() == null) return;

        Gizmos.color = Color.red;
        Vector2 playerSize = GetComponent<Collider2D>().bounds.size;
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(transform.localScale.x * (playerSize.x / 2 + meleeAttackOffset), 0);
        Vector2 attackSize = new Vector2(playerSize.x * 1.5f, playerSize.y * 0.7f);

        Gizmos.DrawWireCube(attackCenter, attackSize);
    }
}