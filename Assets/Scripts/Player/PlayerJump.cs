
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float jumpForce = 12f;

    [SerializeField, Range(0f, 1f)]
    private float jumpCutMultiplier = 0.5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
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
