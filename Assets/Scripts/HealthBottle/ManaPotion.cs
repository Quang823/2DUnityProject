using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    public float manaAmount = 50f; // L??ng mana h?i khi nh?t bình
    public AudioClip pickupSound;  // Âm thanh khi nh?t

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ki?m tra n?u là Player
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.RestoreMana(manaAmount); // H?i n?ng l??ng
            }

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject); // Xóa bình n?ng l??ng sau khi nh?t
        }
    }
}
