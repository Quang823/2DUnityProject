using UnityEngine;

public class FlagTrigger : MonoBehaviour
{
    public MovingPlatforms platform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && platform != null)
        {
            platform.SetPlatformActive(true); 
            collision.transform.SetParent(platform.transform); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && platform != null)
        {
            platform.SetPlatformActive(false); 
            collision.transform.SetParent(null);
        }
    }
}
