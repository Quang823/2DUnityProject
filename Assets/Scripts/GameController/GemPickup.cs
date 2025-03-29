using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GemPickup : MonoBehaviour
{
    public Sprite gemSprite;
    public BossHealth bossHealth;
    public Transform groundCheck; 
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isCollected = false;
    private bool hasLanded = false;
    public int levelIndex;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!hasLanded && IsOnGround())
        {
            LandOnGround();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            if (GemManager.instance != null)
            {
                GemManager.instance.AddGem(gemSprite, levelIndex);
            }

            gameObject.SetActive(false);

            if (bossHealth != null)
            {
                Invoke(nameof(DelayTransition), 0.5f);
            }
        }
    }


    private void DelayTransition()
    {
        bossHealth.OnGemCollected();
    }



    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void LandOnGround()
    {
        hasLanded = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    void AddGemToUI()
    {
        GameObject[] gemSlots = GameObject.FindGameObjectsWithTag("GemSlot");

        if (gemSlots.Length == 0)
        {
            return;
        }

        foreach (GameObject slot in gemSlots)
        {
            Image slotImage = slot.GetComponent<Image>();

            if (slotImage != null && slotImage.sprite == null)
            {
                slotImage.sprite = gemSprite;
                slotImage.color = Color.white;
                return;
            }
        }
    }
}
