using UnityEngine;

public class PlayerAttack2 : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement123 playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
       anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement123>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();
        

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldownTimer = 0; 

    }

}
