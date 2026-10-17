using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidade")]
    [SerializeField, Min(0f)] private float moveSpeed = 10f;

    [Header("Aceleração")]
    [SerializeField, Min(0f)] private float acceleration = 75f;
    [SerializeField, Min(0f)] private float deceleration = 90f;
    [SerializeField, Range(0f, 1f)] private float airMultiplier = 0.65f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveHorizontal(float input, bool inAir = false)
    {
        float targetVelocityX = input * moveSpeed;
        float rate = (Mathf.Abs(input) > 0.01f) ? acceleration : deceleration;

        if (inAir)
        {
            rate *= airMultiplier;
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, rate * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }

    public void StopHorizontal()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0f, deceleration * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }
}