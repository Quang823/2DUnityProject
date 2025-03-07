using UnityEngine;

public class PlayerRound2 : MonoBehaviour
{
    private static PlayerRound2 instance;
    public static PlayerRound2 Instance { get { return instance; } }

    private Rigidbody2D rb;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();

        // Đảm bảo Rigidbody2D là Dynamic để chịu trọng lực
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Chỉ khóa xoay, cho phép di chuyển
    }

    void Start()
    {
        // Vô hiệu hóa va chạm vật lý giữa Enemy và Player
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Quái tấn công Player!");

            // Tạm thời vô hiệu hóa lực đẩy của quái lên Player
            rb.linearVelocity = Vector2.zero;
        }
    }
}
