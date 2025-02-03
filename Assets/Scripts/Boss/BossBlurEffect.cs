using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BossBlurEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        // L?y hi?u ?ng Depth of Field t? Post Process Volume
        if (postProcessVolume.profile.TryGetSettings(out depthOfField))
        {
            depthOfField.active = false; // M?c ??nh không làm m?
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss")) // Khi Boss xu?t hi?n
        {
            depthOfField.active = true; // B?t hi?u ?ng làm m?
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boss")) // Khi Boss bi?n m?t
        {
            depthOfField.active = false; // T?t hi?u ?ng làm m?
        }
    }
}
