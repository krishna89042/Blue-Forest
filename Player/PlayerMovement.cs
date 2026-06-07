using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 12f;
    public float deceleration = 15f;
    public float airControlMultiplier = 0.8f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float gravityScale = 3f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Jump Forgiveness")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    private Rigidbody2D rb;
    private float moveInput;
    private float velocityXSmoothing;
    private bool isGrounded;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private SpriteRenderer spriteRenderer;
    Animator animator;
    [Header("Control")]
    //public bool canMove = true;
    public bool allowInput = true;
    [Header("Auto Movement")]
    public bool autoMove = false;
    public float autoMoveSpeed = 0f;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityScale;
    }


    void Update()
    {
        /* if (!canMove)
         {
             rb.linearVelocity = Vector2.zero;
             animator.SetFloat("Speed", 0);
             return;
         }*/
        if (autoMove)
        {
            moveInput = autoMoveSpeed;
        }
        else if (!allowInput)
        {
            moveInput = 0;
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        //   moveInput = Input.GetAxisRaw("Horizontal");

        // Flip sprite
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ANIMATION PARAMETERS
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);

        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump logic
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0;
        }

        // Better jump feel
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float smoothTime = isGrounded ? acceleration : acceleration * airControlMultiplier;

        float newVelocityX = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, smoothTime * Time.fixedDeltaTime);

        // Deceleration when no input
        if (moveInput == 0 && isGrounded)
        {
            newVelocityX = Mathf.Lerp(rb.linearVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
    }

    // Draw ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}