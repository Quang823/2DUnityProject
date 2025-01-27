using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpForce = 20f;

    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float climbSpeed = 10f;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private Vector3 initialScale;
    private float horizontalInput;
    private bool isClimbing;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialScale = transform.localScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * climbSpeed);
        }
        else
        {

            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (horizontalInput > 0.01f)
            {
                transform.localScale = initialScale;
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
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

            LimitPlayerPosition();
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

    private void LimitPlayerPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            body.gravityScale = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            body.gravityScale = 1f;
        }
    }
}
