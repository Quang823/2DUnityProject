using System.Collections;
using UnityEngine;

public class PlayerMovementR3 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 10f;
    public float acceleration = 50f;
    public float deceleration = 30f;
    public float airControl = 0.5f;

    [Header("Jump Settings")]
    public float jumpForce = 14f;
    public float jumpHoldForce = 4f;
    public float jumpTime = 0.2f;
    private float jumpTimeCounter;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.5f;
    private float lastDashTime = -10f;
    public float dashManaCost = 5f;

    [Header("References")]
    public GameObject dashEffectObject;
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private PlayerStats playerStats;

    [Header("State Variables")]
    private bool grounded;
    private bool isJumping;
    private bool isDashing;
    private float horizontalInput;
    private Vector3 initialScale;
    private float dashEndTime;
    private float originalGravityScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialScale = transform.localScale;
        originalGravityScale = body.gravityScale;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            HoldJump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.L) && Time.time >= lastDashTime + dashCooldown)
        {
            Dash();
        }
    }

    private void HandleMovement()
    {
        if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                body.gravityScale = originalGravityScale; // Khôi phục lại giá trị gravityScale ban đầu
                dashEffectObject.SetActive(false);
            }
            return;
        }

        float targetSpeed = horizontalInput * speed;
        float speedDifference = targetSpeed - body.linearVelocity.x;
        float accelerationRate = grounded ? acceleration : acceleration * airControl;
        float movementForce = speedDifference * accelerationRate;

        body.AddForce(Vector2.right * movementForce);

        if (horizontalInput > 0)
            transform.localScale = initialScale;
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        isJumping = true;
        jumpTimeCounter = jumpTime;
        anim.SetTrigger("jump");
    }

    private void HoldJump()
    {
        if (jumpTimeCounter > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce + jumpHoldForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    public bool canAttack()
    {
        return grounded && horizontalInput == 0;
    }

    private void Dash()
    {
        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        body.linearVelocity = new Vector2(dashForce * dashDirection, body.linearVelocity.y);
        isDashing = true;
        dashEndTime = Time.time + dashTime;
        //body.gravityScale = originalGravityScale / 2; // Giảm trọng lực một nửa
        body.gravityScale = 0;
        dashEffectObject.SetActive(false);
        dashEffectObject.transform.position = transform.position;
        dashEffectObject.SetActive(true);

        playerStats.UseSkill(dashManaCost);
        lastDashTime = Time.time;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
