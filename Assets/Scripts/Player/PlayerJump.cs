using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField, Min(0f)] private float jumpForce = 14f;
    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

    [Header("Game Feel")]
    [SerializeField, Min(0f)] private float coyoteTime = 0.12f;
    [SerializeField, Min(0f)] private float jumpBufferTime = 0.12f;

    [Header("Wall Jump")]
    [SerializeField, Min(0f)] private float wallJumpForceY = 22f;
    [SerializeField, Min(0f)] private float wallLeapForceX = 14f;
    [SerializeField, Min(0f)] private float wallClimbForceX = 4f;
    [SerializeField, Min(0f)] private float wallCoyoteTime = 0.15f;

    [Header("Slide Jump (Pulo Turbinado)")]
    [SerializeField, Min(0f)] private float slideJumpSpeedX = 16f;
    [SerializeField, Min(0f)] private float slideJumpForceY = 20f;

    private float wallCoyoteTimer;
    public int LastWallDirection { get; private set; }
    public bool CanWallJump => wallCoyoteTimer > 0f && jumpBufferTimer > 0f;

    private Rigidbody2D rb;
    private Player player;

    private float coyoteTimer;
    private float jumpBufferTimer;

    public float VerticalVelocity => rb.linearVelocity.y;
    public bool CanJump => coyoteTimer > 0f && jumpBufferTimer > 0f;
    public bool HasBufferedJump => jumpBufferTimer > 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.InputHandler.JumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (player.Collision.IsGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        if (player.Collision.IsTouchingWall && !player.Collision.IsGrounded)
        {
            wallCoyoteTimer = wallCoyoteTime;
            LastWallDirection = player.Collision.WallDirection;
        }
        else
        {
            wallCoyoteTimer -= Time.deltaTime;
        }
    }

    public void Jump()
    {
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;

        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    public void CutJump()
    {
        if (rb.linearVelocity.y <= 0f)
            return;

        Vector2 velocity = rb.linearVelocity;
        velocity.y *= jumpCutMultiplier;
        rb.linearVelocity = velocity;
    }

    public void WallJump(int wallDir, float horizontalInput)
    {
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        wallCoyoteTimer = 0f;

        bool climbingSameWall = (horizontalInput != 0 && Mathf.Sign(horizontalInput) == wallDir);
        float forceX = climbingSameWall ? wallClimbForceX : wallLeapForceX;

        Vector2 velocity = rb.linearVelocity;
        velocity.x = -wallDir * forceX;
        velocity.y = wallJumpForceY;
        rb.linearVelocity = velocity;
    }

    public void SlideJump(float direction)
    {
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = direction * slideJumpSpeedX;
        velocity.y = slideJumpForceY;
        rb.linearVelocity = velocity;
    }
}