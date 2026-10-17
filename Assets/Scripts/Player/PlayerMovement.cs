
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float moveSpeed = 8f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveHorizontal(float input)
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = input * moveSpeed;
        rb.linearVelocity = velocity;
    }

    public void StopHorizontal()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = 0f;
        rb.linearVelocity = velocity;
    }
}
