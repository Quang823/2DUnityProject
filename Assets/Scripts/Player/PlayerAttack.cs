using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip meleeAttackSound;
    [SerializeField] private AudioClip fireAttackSound;

    [Header("General Settings")]
    [SerializeField] private float meleeattackCooldown;
    [SerializeField] private float farattackCooldown;
    private float cooldownTimer = Mathf.Infinity;
    private float meleecooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

    [Header("Melee Attack Settings")]
    [SerializeField] private float meleeAttackOffset = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int meleeAttackDamage;

    [Header("Far Attack Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] farattack;
    [SerializeField] private float manaCost;

    [Header("Blocking Settings")]
    [SerializeField] private GameObject shieldEffect;
    public bool isBlocking = false;
    [SerializeField] private float blockDuration = 1.5f;
    [SerializeField] private float blockCooldown = 2f;
    private float lastBlockTime = Mathf.NegativeInfinity;

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

    private void Start()
    {
        isBlocking = false;
        shieldEffect.SetActive(false);
    }


    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        meleecooldownTimer += Time.deltaTime; 

        if (Input.GetKeyDown(KeyCode.J) && playerMovement.canAttack() && meleecooldownTimer >= meleeattackCooldown)
        {
            meleecooldownTimer = 0;
            MeleeAttack();
        }


        if (Input.GetKeyDown(KeyCode.K) && playerMovement.canAttack() && cooldownTimer >= farattackCooldown)
        {
            if (playerStats.CanUseSkill(manaCost))
            {
                cooldownTimer = 0; 
                FireAttack();
                playerStats.UseSkill(manaCost);
            }
        }

        if (Input.GetKeyDown(KeyCode.H) && Time.time - lastBlockTime >= blockCooldown)
        {
            StartBlocking();
        }
    }



    private void MeleeAttack()
    {
        anim.SetTrigger("attack");

  
        if (meleeAttackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(meleeAttackSound);
        }
    }



    private void DealMeleeDamage()
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(meleeAttackOffset * Mathf.Sign(transform.localScale.x), 0);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPosition, 0.5f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(meleeAttackDamage);
            }
            else if (enemy.TryGetComponent<EnemyHealthRound3>(out EnemyHealthRound3 enemyHealthRound3))
            {
                enemyHealthRound3.TakeDamage(meleeAttackDamage);
            }
            else if (enemy.TryGetComponent<BossHealth>(out BossHealth bossHealth))
            {
                bossHealth.TakeDamage(meleeAttackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 attackPosition = (Vector2)transform.position + new Vector2(meleeAttackOffset * Mathf.Sign(transform.localScale.x), 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, 0.5f);
    }

    private void FireAttack()
    {
        anim.SetTrigger("fireattack");
        cooldownTimer = 0;

        // Phát âm thanh đánh xa
        if (fireAttackSound != null)
        {
            audioSource.PlayOneShot(fireAttackSound);
        }

        if (farattack.Length > 0 && farattack[currentAttackIndex] != null)
        {
            GameObject projectile = farattack[currentAttackIndex];
            projectile.transform.position = firePoint.position;
            projectile.GetComponent<ProjecTile>().SetDirection(Mathf.Sign(transform.localScale.x));
            projectile.SetActive(true);
            currentAttackIndex = (currentAttackIndex + 1) % farattack.Length;
        }
    }

    private void StartBlocking()
    {
        isBlocking = true;
        lastBlockTime = Time.time;
        shieldEffect.SetActive(true);
        Invoke(nameof(StopBlocking), blockDuration);
    }

    private void StopBlocking()
    {
        isBlocking = false;
        shieldEffect.SetActive(false);
    }
}
