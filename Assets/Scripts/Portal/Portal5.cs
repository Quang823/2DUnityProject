using UnityEngine;

public class Portal5 : MonoBehaviour
{
    public Transform bossArenaSpawnPoint; // ?i?m d?ch chuy?n ??n ??u tr??ng Boss

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ki?m tra n?u va ch?m v?i Player
        {
            other.transform.position = bossArenaSpawnPoint.position; // D?ch chuy?n Player
        }
    }
}
