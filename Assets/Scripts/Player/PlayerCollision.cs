
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Parede")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField, Min(0.01f)] private float wallCheckDistance = 0.55f;
    public bool IsTouchingWall { get; private set; }
    public int WallDirection { get; private set; }

    [SerializeField] private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField, Min(0.01f)] private float groundCheckRadius = 0.12f;
    [SerializeField, Min(0.01f)] private float ceilingCheckDistance = 0.55f;

    public bool IsGrounded { get; private set; }
    public bool HasCeilingAbove { get; private set; }
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }  

        private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        Vector2 rayOrigin = (Vector2)transform.position + (circleCollider != null ? circleCollider.offset : Vector2.zero);
        RaycastHit2D hitCeiling = Physics2D.Raycast(rayOrigin, Vector2.up, ceilingCheckDistance, groundLayer);
        HasCeilingAbove = (hitCeiling.collider != null);

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);

        if (hitRight.collider != null)
        {
            IsTouchingWall = true;
            WallDirection = 1;
        }
        else if (hitLeft.collider != null)
        {
            IsTouchingWall = true;
            WallDirection = -1;
        }
        else
        {
            IsTouchingWall = false;
            WallDirection = 0;
        }
    }

    public void SetCrouchCollider(bool crouch)
    {
        if (circleCollider == null) return;
        if (crouch)
        {
            circleCollider.radius = 0.375f;
            circleCollider.offset = new Vector2(0f, -0.375f);
        }
        else
        {
            circleCollider.radius = 0.75f;
            circleCollider.offset = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
    }
}
