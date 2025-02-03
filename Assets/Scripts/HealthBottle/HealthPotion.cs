using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healAmount = 50f; // S? máu h?i khi nh?t bình
    public AudioClip pickupSound;  // Âm thanh khi nh?t

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ki?m tra n?u là Player
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Heal(healAmount); // H?i máu
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject); // Xóa bình máu sau khi nh?t
        }
    }
}
