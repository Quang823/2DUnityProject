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
    public GameObject[] gems;
    public Transform dropPoint;
    public Image fadeScreen;

    public string menuSceneName = "MAIN MENU";

    private bool isDead = false;
    private bool gemCollected = false;

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
                rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 5f)), ForceMode2D.Impulse);
            }
        }

        gameObject.SetActive(false);
    }

    public void OnGemCollected()
    {
        if (gemCollected) return;
        gemCollected = true;

        if (GameController.instance != null)
        {
            GameController.instance.StartGlobalCoroutine(FadeAndReturnToMenu());
        }
        else
        {
            Debug.LogError("GameController.instance is NULL!");
        }
    }

    private IEnumerator FadeAndReturnToMenu()
    {
        if (fadeScreen == null) yield break;

        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            fadeScreen.color = new Color(0, 0, 0, t / 1.5f);
            yield return null;
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            Debug.LogWarning("No player found with tag 'Player'");
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(menuSceneName);
    }
}
