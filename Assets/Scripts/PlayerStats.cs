using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Elements")]
    public Image healthBar; 
    public Image manaBar;
    public TextMeshProUGUI  healthText;
    public TextMeshProUGUI manaText;   

    [Header("Player Stats")]
    private float maxHealth = 200f;
    private float currentHealth;

    private float maxMana = 200f;
    private float currentMana;

    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        UpdateHealthUI();
        UpdateManaUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UseSkill(float manaCost)
    {
        if (CanUseSkill(manaCost))
        {
            currentMana = Mathf.Clamp(currentMana - manaCost, 0, maxMana);
            UpdateManaUI();
            Debug.Log("Skill used!");
        }
        else
        {
            Debug.Log("Not enough mana!");
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
}
