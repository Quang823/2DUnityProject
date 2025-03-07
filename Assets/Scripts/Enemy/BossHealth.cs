using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Rewards")]
    public GameObject[] gems; // 5 viên ngọc
    public Transform dropPoint; // Vị trí rơi ngọc
    public CanvasGroup fadeScreen; // Hiệu ứng màn hình đen
    public string menuSceneName = "MainMenu"; // Tên scene menu

    private bool isDead = false;
    private bool gemCollected = false; // Kiểm tra xem viên ngọc đã được nhặt chưa

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

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("dead");
        }

        healthBar.transform.parent.gameObject.SetActive(false);

        StartCoroutine(HandleBossDeath());
    }

    private IEnumerator HandleBossDeath()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        healthBar.transform.parent.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;

        if (gems.Length > 0)
        {
            int randomIndex = Random.Range(0, gems.Length);
            Vector3 bossPosition = transform.position;
            GameObject droppedGem = Instantiate(gems[randomIndex], bossPosition, Quaternion.identity);
            droppedGem.GetComponent<GemPickup>().bossHealth = this;
            droppedGem.SetActive(true);

            Rigidbody2D rb = droppedGem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                float randomForceX = Random.Range(-1f, 1f);
                float randomForceY = Random.Range(3f, 5f);
                rb.AddForce(new Vector2(randomForceX, randomForceY), ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(0.5f);
            AdjustGemPosition(droppedGem);
        }

        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void AdjustGemPosition(GameObject gem)
    {
        Collider2D ground = Physics2D.OverlapCircle(gem.transform.position, 1f, LayerMask.GetMask("Ground"));
        if (ground != null)
        {
            Vector3 newPos = ground.bounds.center;
            newPos.y = ground.bounds.max.y + 0.2f; 
            gem.transform.position = newPos;

            Rigidbody2D rb = gem.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    public void OnGemCollected()
    {
        gemCollected = true;
        GameController.instance.StartGlobalCoroutine(FadeAndReturnToMenu());

    }

    private IEnumerator FadeAndReturnToMenu()
    {
        if (fadeScreen != null)
        {
            float fadeDuration = 1.5f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                fadeScreen.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            fadeScreen.alpha = 1;
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(menuSceneName);
    }
}
