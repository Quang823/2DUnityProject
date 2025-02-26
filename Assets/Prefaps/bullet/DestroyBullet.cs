using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // L??ng sát th??ng c?a viên ??n

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // N?u ??n va ch?m v?i Player
        {
            PlayerStats player = collision.GetComponent<PlayerStats>(); // L?y PlayerStats
            if (player != null)
            {
                player.TakeDamage(damage); // Gây sát th??ng cho Player
            }
            Destroy(gameObject); // Xóa viên ??n sau khi b?n trúng
        }
        else if (collision.CompareTag("Ground")) // N?u ??n ch?m vào ??t
        {
            Destroy(gameObject);
        }
    }
}
