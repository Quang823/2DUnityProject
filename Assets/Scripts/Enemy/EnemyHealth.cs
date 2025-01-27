using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
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

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("dead");
        }

        healthBar.transform.parent.gameObject.SetActive(false);
        AdjustPositionToGround();
        StartCoroutine(HideAndDestroy());
    }

    private void AdjustPositionToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + (GetComponent<Collider2D>().bounds.size.y / 2);
            transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("Enemy is not above the ground! Using fallback position.");
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z); 
        }

    }

    private System.Collections.IEnumerator HideAndDestroy()
    {     
        float animationLength = 2f; 
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
        gameObject.SetActive(false);       
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
