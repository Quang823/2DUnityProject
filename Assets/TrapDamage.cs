using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public float damage = 5f;  // Lượng máu trừ khi chạm vào bẫy
    public float damageInterval = 1f; // Thời gian giữa mỗi lần trừ máu
    private bool playerInTrap = false; // Kiểm tra xem nhân vật có đang đứng trong bẫy không
    private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                playerInTrap = true;
                InvokeRepeating(nameof(ApplyContinuousDamage), damageInterval, damageInterval);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrap = false;
            CancelInvoke(nameof(ApplyContinuousDamage));
        }
    }

    private void ApplyContinuousDamage()
    {
        if (playerInTrap && playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }
}
