﻿using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] farattack;
    [SerializeField] private float manaCost = 20f; 
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats; 
    private float cooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

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
            else
            {
                Debug.Log("Not enough mana to use FireAttack!");
            }
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
        else
        {
            Debug.LogWarning("Projectile is null or farattack array is empty.");
        }
    }
}
