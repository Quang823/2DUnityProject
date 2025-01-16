using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private Vector3 initialScale;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialScale = transform.localScale;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = initialScale;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            Jump();
        }

        bool isRunning = horizontalInput != 0;
        if (anim.GetBool("run") != isRunning)
        {
            anim.SetBool("run", isRunning);
        }

        if (anim.GetBool("grounded") != grounded)
        {
            anim.SetBool("grounded", grounded);
        }
    }

    public void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    public bool canAttack()
    {
        return grounded && horizontalInput == 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
