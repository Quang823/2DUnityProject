using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 20f;
    public float jumpForce = 20f;
    public float climbSpeed = 5f;

    [Header("Dash Settings")]
    public float dashForce = 150f;
    private float dashTime = 0.3f;
    public float dashCooldown = 3f;
    private float lastDashTime = -10f;
    public float dashManaCost = 5f;

    [Header("Boundary Settings")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("References")]
    public GameObject dashEffectObject;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PlayerStats playerStats;

    [Header("State Variables")]
    private bool grounded;
    private bool isClimbing;
    private bool isDashing = false;
    private float horizontalInput;
    private Vector3 initialScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        initialScale = transform.localScale;
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        LimitPlayerPosition();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.L) && Time.time >= lastDashTime + dashCooldown && playerStats.CanUseSkill(dashManaCost))
        {
            Dash();
        }
    }

    private void HandleMovement()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * climbSpeed);
            anim.SetBool("isClimbing", verticalInput != 0);
            anim.SetBool("grounded", false);
        }
        else if (!isDashing)
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

            anim.SetBool("run", horizontalInput != 0);
            anim.SetBool("grounded", grounded);
            anim.SetBool("isClimbing", false);
        }
    }

    public bool canAttack()
    {
        return grounded && horizontalInput == 0;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private void Dash()
    {
        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        body.linearVelocity = new Vector2(dashForce * dashDirection, body.linearVelocity.y);
        isDashing = true;

        dashEffectObject.SetActive(false);
        dashEffectObject.transform.position = transform.position;
        dashEffectObject.SetActive(true);

        playerStats.UseSkill(dashManaCost);
        lastDashTime = Time.time;

        StartCoroutine(StopDash());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        body.linearVelocity = Vector2.zero;
        dashEffectObject.SetActive(false);
        isDashing = false;
    }

    private void LimitPlayerPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            body.gravityScale = 0f;
            anim.SetBool("isClimbing", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            body.gravityScale = 1f;
            anim.SetBool("isClimbing", false);
        }
    }
}
