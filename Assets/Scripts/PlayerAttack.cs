using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] farattack;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && playerMovement.canAttack())
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.K) && playerMovement.canAttack())
        {
            FireAttack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack"); 
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
            projectile.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
            projectile.SetActive(true); 
            currentAttackIndex = (currentAttackIndex + 1) % farattack.Length; 
        }
     
    }
}
