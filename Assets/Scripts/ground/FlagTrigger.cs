using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    public MovingPlatform platform; // Tham chi?u ??n b?c th?m

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            platform.SetPlatformActive(true); // B?t ch? ?? di chuy?n lên
            collision.transform.parent = platform.transform; // G?n player vào platform
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            platform.SetPlatformActive(false); // T?t ch? ?? di chuy?n lên
            collision.transform.parent = null; // B? liên k?t player v?i platform
        }
    }
}
