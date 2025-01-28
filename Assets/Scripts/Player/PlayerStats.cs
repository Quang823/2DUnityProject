using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public Image manaBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    [Header("Player Stats")]
    private float maxHealth = 200f;
    private float currentHealth;

    private float maxMana = 200f;
    private float currentMana;

    [Header("Hurt Effect")]
    public SpriteRenderer spriteRenderer; 
    public Animator animator;
    private bool isInvincible = false;
    private bool isHurting = false;
    //[SerializeField] private Transform respawnPoint;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float horizontalMove;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        UpdateHealthUI();
        UpdateManaUI();
    }

    private void Attack()
    {
        if (animator != null && !isHurting)
        {
            animator.SetTrigger("attack");
        }
    }

    private void Update()
    {
        if (!isInvincible && currentHealth > 0)
        {
            animator.SetBool("isHurt", false); 
        }

        if (!isHurting)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
            }
        }
    }

    public void TakeDamage(float damage = 10)
    {
        if (isInvincible) return; 

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();

        if (animator != null)
        {
            animator.SetTrigger("hurt");
            animator.SetBool("isHurt", true); 
        }
        StartCoroutine(HurtEffect());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UseSkill(float manaCost = 10)
    {
        if (CanUseSkill(manaCost))
        {
            currentMana = Mathf.Clamp(currentMana - manaCost, 0, maxMana);
            UpdateManaUI();
        }
 
    }

    public void UseDash(float manaDashCost)
    {
        if (CanUseSkill(manaDashCost))
        {
            currentMana = Mathf.Clamp(currentMana - manaDashCost, 0, maxMana);
            UpdateManaUI();
        }

    }

    public bool CanUseSkill(float manaCost)
    {
        return currentMana >= manaCost;
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
        }
    }

    private void UpdateManaUI()
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
        }

        if (manaText != null)
        {
            manaText.text = $"{Mathf.RoundToInt(currentMana)}/{Mathf.RoundToInt(maxMana)}";
        }
    }

    private IEnumerator HurtEffect()
    {
        isHurting = true;
        isInvincible = true; 
        float duration = 1f; 
        float blinkInterval = 0.1f; 

        for (float t = 0; t < duration; t += blinkInterval)
        {
            spriteRenderer.color = Color.red; 
            yield return new WaitForSeconds(blinkInterval / 2);

            spriteRenderer.color = Color.white; 
            yield return new WaitForSeconds(blinkInterval / 2);
        }

        spriteRenderer.color = Color.white; 
        isInvincible = false; 
        isHurting = false;
    }

    private void Die()
    {
        healthBar.transform.parent.gameObject.SetActive(false);
        manaBar.transform.parent.gameObject.SetActive(false);
        StartCoroutine(DeathEffect());
        //StartCoroutine(Respawn());
    }

    private IEnumerator DeathEffect()
    {
        isInvincible = true; 
        float duration = 3f;
        float blinkInterval = 0.2f;

        for (float t = 0; t < duration; t += blinkInterval)
        {
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(blinkInterval / 2);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkInterval / 2);
        }

        spriteRenderer.color = Color.clear; 
        gameObject.SetActive(false);
    }

    //private IEnumerator Respawn()
    //{
    //    spriteRenderer.color = Color.clear; 
    //    yield return new WaitForSeconds(7f); 
    //    currentHealth = maxHealth; 
    //    UpdateHealthUI();
    //    transform.position = respawnPoint.position; 
    //    spriteRenderer.color = Color.white;
    //}
}
