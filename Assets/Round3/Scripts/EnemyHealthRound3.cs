using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthRound3 : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public TextMeshProUGUI healthText; 

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Animation & Effects")]
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyPatrol enemyPatrol; 
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();

        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    private bool isDead = false;

    private void Die()
    {
        if (isDead) return;  

        isDead = true; 

        if (animator != null)
        {
            Debug.Log("Die");
            animator.ResetTrigger("hurt");
            animator.SetTrigger("dead");
        }

        healthBar.transform.parent.gameObject.SetActive(false);
        GameController.instance.EnemyDefeated();
    }

    private void HideAndDestroy()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
