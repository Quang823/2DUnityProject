using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    public MovingPlatform platform; // Tham chi?u ??n b?c th?m/platform

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && platform != null)
        {
            platform.SetPlatformActive(true); // B?t ch? ?? di chuy?n lên
            collision.transform.SetParent(platform.transform); // Gán player vào platform
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && platform != null)
        {
            platform.SetPlatformActive(false); // T?t ch? ?? di chuy?n
            collision.transform.SetParent(null); // B? liên k?t player v?i platform
        }
    }
}
