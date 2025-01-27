using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float attackCooldown; 
    private float cooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackRange; 
    [SerializeField] private LayerMask enemyLayer; 
    [SerializeField] private int meleeAttackDamage; 

    [Header("Far Attack Settings")]
    [SerializeField] private Transform firePoint; 
    [SerializeField] private GameObject[] farattack; 
    [SerializeField] private float manaCost; 

    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && playerMovement.canAttack())
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.K) && playerMovement.canAttack())
        {
            if (playerStats.CanUseSkill(manaCost))
            {
                FireAttack();
                playerStats.UseSkill(manaCost);
            }
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, meleeAttackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(meleeAttackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, meleeAttackRange);
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
}