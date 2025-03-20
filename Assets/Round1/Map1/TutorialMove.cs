using UnityEngine;
using UnityEngine.UI;

public class TutorialMove : MonoBehaviour
{
    public Image keyImageA, keyImageD; // Hình ảnh phím A và D
    public Sprite spriteA, spriteD;    // Sprite của phím A và D
    public GameObject nextTrigger;     // Trigger cho phần nhảy
    private float speed = 5f;          // Tốc độ di chuyển
    private bool canMove = false;      // Kiểm soát khi nào di chuyển
    private Rigidbody2D rb;            // Rigidbody của Player

    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canMove = true;
            keyImageA.sprite = spriteA;
            keyImageD.sprite = spriteD;
            keyImageA.gameObject.SetActive(true);
            keyImageD.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (canMove)
        {
            float moveInput = Input.GetAxisRaw("Horizontal"); // A/D hoặc mũi tên
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

            // Nhấp nháy phím
            BlinkKeys(keyImageA);
            BlinkKeys(keyImageD);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Goal")) // Khi chạm Goal
        {
            keyImageA.gameObject.SetActive(false);
            keyImageD.gameObject.SetActive(false);
            nextTrigger.SetActive(true); // Kích hoạt phần nhảy
            canMove = false;
        }
    }

    void BlinkKeys(Image key)
    {
        key.enabled = Mathf.Sin(Time.time * 5f) > 0; // Nhấp nháy nhanh
    }
}