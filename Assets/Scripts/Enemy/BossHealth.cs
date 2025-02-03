using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar;
    public TextMeshProUGUI healthText;

    [Header("Stats")]
    [SerializeField] private float maxHealth = 20000f;
    private float currentHealth;

    [Header("Animation & Effects")]
    [SerializeField] private Animator animator;
    [SerializeField] private BossPatrol bossPatrol;
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();

        if (animator != null)
        {
            animator.SetTrigger("hurt");
        }

        if (bossPatrol != null)
        {
            bossPatrol.enabled = false;
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
            animator.SetTrigger("dead");
        }

        healthBar.transform.parent.gameObject.SetActive(false);
        GameController.instance.EnemyDefeated();
        StartCoroutine(HideAndDestroy());
    }

    private System.Collections.IEnumerator HideAndDestroy()
    {
        float animationLength = 1f;
        if (animator != null)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == "dead")
                {
                    animationLength = clip.length;
                    break;
                }
            }
        }

        yield return new WaitForSeconds(animationLength);
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
