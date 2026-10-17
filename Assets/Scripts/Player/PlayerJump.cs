using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Pulo")]
    [SerializeField, Min(0f)] private float jumpForce = 14f;
    [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

    [Header("Tolerâncias")]
    [SerializeField, Min(0f)] private float coyoteTime = 0.12f;
    [SerializeField, Min(0f)] private float jumpBufferTime = 0.12f;

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
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (player.Collision.IsGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
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
}