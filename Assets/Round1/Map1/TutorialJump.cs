using UnityEngine;
using UnityEngine.UI;

public class TutorialJump : MonoBehaviour
{
    public Image keyImageSpace;    // H�nh ?nh ph�m Space
    public Sprite spriteSpace;     // Sprite c?a Space
    public float jumpForce = 5f;   // ?? cao nh?y
    private Rigidbody2D rb;        // Rigidbody c?a Player
    private bool canJump = false;  // Ki?m so�t khi n�o nh?y

    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canJump = true;
            keyImageSpace.sprite = spriteSpace;
            keyImageSpace.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Nh?y
        }
        if (canJump) BlinkKeys(keyImageSpace);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            keyImageSpace.gameObject.SetActive(false);
            canJump = false;
        }
    }

    void BlinkKeys(Image key)
    {
        key.enabled = Mathf.Sin(Time.time * 5f) > 0; // Nh?p nh�y nhanh
    }
}