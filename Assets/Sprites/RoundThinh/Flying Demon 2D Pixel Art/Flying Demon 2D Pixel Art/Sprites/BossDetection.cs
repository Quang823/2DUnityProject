using UnityEngine;

public class BossDetection : MonoBehaviour
{
    public BossAimShoot bossScript; // Tham chi?u ??n script b?n ??n c?a Boss

    void Start()
    {
        bossScript.enabled = false; // Ban ??u t?t BossAttack
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // N?u Player ?i vào vùng
        {
            bossScript.enabled = true; // Kích ho?t b?n
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // N?u Player r?i kh?i vùng
        {
            bossScript.enabled = false; // D?ng b?n
        }
    }
}
