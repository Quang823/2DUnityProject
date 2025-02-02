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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

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

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.SetTrigger("hurt");
            StartCoroutine(HurtEffect());
        }

        if (currentHealth <= 0) Die();
    }


    public void UseSkill(float manaCost = 10)
    {
        if (CanUseSkill(manaCost))
        {
            currentMana = Mathf.Clamp(currentMana - manaCost, 0, maxMana);
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
        float duration = 0.5f;
        float blinkInterval = 0.05f;

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

    private bool isDead = false; 

    private void Die()
    {
        if (isDead) return; 

        isDead = true; 
        isInvincible = true; 

        healthBar.transform.parent.gameObject.SetActive(false);
        manaBar.transform.parent.gameObject.SetActive(false);

        StartCoroutine(DeathEffect());

        GameController.instance.PlayerDied();
    }

    private IEnumerator DeathEffect()
    {
        float duration = 1f;
        float blinkInterval = 0.1f;

        for (float t = 0; t < duration; t += blinkInterval)
        {
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(blinkInterval / 2);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkInterval / 2);
        }

        spriteRenderer.color = Color.clear;
        gameObject.SetActive(false); // Biến mất hoàn toàn
    }

    public void Respawn(Vector2 checkpointPosition)
    {
        gameObject.SetActive(true);
        isDead = false; // Reset trạng thái chết
        isInvincible = true; // Hồi sinh xong vẫn được miễn damage 1 thời gian

        Vector2 spawnPosition = checkpointPosition + Vector2.up * 2f;
        transform.position = spawnPosition;

        currentHealth = maxHealth;
        currentMana = maxMana;
        UpdateHealthUI();
        UpdateManaUI();

        spriteRenderer.color = Color.white;
        healthBar.transform.parent.gameObject.SetActive(true);
        manaBar.transform.parent.gameObject.SetActive(true);

        StartCoroutine(FallToCheckpoint(checkpointPosition));
    }


    private IEnumerator FallToCheckpoint(Vector2 checkpointPosition)
    {
        isInvincible = true;

        float fallSpeed = 5f;
        float blinkDuration = 1f;
        float blinkInterval = 0.1f;

        // Nhấp nháy trong khi rơi
        StartCoroutine(BlinkEffect(blinkDuration, blinkInterval));

        while (transform.position.y > checkpointPosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, checkpointPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = checkpointPosition;
        isInvincible = false;
    }


    private IEnumerator BlinkEffect(float duration, float interval)
    {
        float timer = 0f;
        while (timer < duration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Ẩn/hiện nhân vật
            yield return new WaitForSeconds(interval);
            timer += interval;
        }
        spriteRenderer.enabled = true; // Đảm bảo nhân vật hiện lại bình thường
    }
}