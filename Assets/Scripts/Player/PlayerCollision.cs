
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

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

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
