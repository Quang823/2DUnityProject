using System.Collections;
using UnityEngine;

public class PlayerMovementR3 : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;

    [Header("Movement Settings")]
    public float speed = 10f;
    public float acceleration = 50f;
    public float deceleration = 30f;
    public float airControl = 0.5f;
    public float climbSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 14f;
    public float jumpHoldForce = 4f;
    public float jumpTime = 0.2f;
    private float jumpTimeCounter;

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
    private bool isJumping;
    private float horizontalInput;
    private Vector3 initialScale;
    public GameObject miniMap;
    public GameObject fullMap;
    private bool isFullMapActive = false;
    private bool isPlayingRunSound = false;
    private float originalGravityScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
        initialScale = transform.localScale;
        originalGravityScale = body.gravityScale;

        body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        miniMap.SetActive(true);
        fullMap.SetActive(false);
    }

    private void Update()
    {
        HandleInput();
        LimitPlayerPosition();
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

        if (Input.GetKeyDown(KeyCode.L) && Time.time >= lastDashTime + dashCooldown && playerStats.CanUseSkill(dashManaCost))
        {
            Dash();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
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
        else if (isDashing)
        {
            if (Time.time >= lastDashTime + dashTime)
            {
                isDashing = false;
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                body.gravityScale = originalGravityScale;
                dashEffectObject.SetActive(false);
            }
        }
        else
        {
            float targetSpeed = horizontalInput * speed;
            float speedDifference = targetSpeed - body.linearVelocity.x;
            float accelerationRate = grounded ? acceleration : acceleration * airControl;
            float movementForce = speedDifference * accelerationRate;

            body.AddForce(Vector2.right * movementForce);

            if (horizontalInput > 0)
                transform.localScale = initialScale;
            else if (horizontalInput < 0)
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);

            HandleRunSound();
        }
    }

    private void HandleRunSound()
    {
        if (horizontalInput != 0 && grounded)
        {
            if (!isPlayingRunSound && runSound != null)
            {
                audioSource.clip = runSound;
                audioSource.loop = true;
                audioSource.Play();
                isPlayingRunSound = true;
            }
        }
        else
        {
            if (isPlayingRunSound)
            {
                audioSource.Stop();
                isPlayingRunSound = false;
            }
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    public bool canAttack()
    {
        return grounded && horizontalInput == 0;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        isJumping = true;
        jumpTimeCounter = jumpTime;
        anim.SetTrigger("jump");

        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
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

    private void Dash()
    {
        float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
        body.linearVelocity = new Vector2(dashForce * dashDirection, body.linearVelocity.y);
        isDashing = true;
        body.gravityScale = 0;
        dashEffectObject.SetActive(false);
        dashEffectObject.transform.position = transform.position;
        dashEffectObject.SetActive(true);

        playerStats.UseSkill(dashManaCost);
        lastDashTime = Time.time;

        if (dashSound != null)
        {
            audioSource.PlayOneShot(dashSound);
        }
    }

    public void ToggleMap()
    {
        isFullMapActive = !isFullMapActive;
        miniMap.SetActive(!isFullMapActive);
        fullMap.SetActive(isFullMapActive);
    }

    private void LimitPlayerPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void ResetRunSound()
    {
        isPlayingRunSound = false;
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
            body.gravityScale = originalGravityScale;
            anim.SetBool("isClimbing", false);
        }
    }
}
