using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidade")]
    [SerializeField, Min(0f)] private float moveSpeed = 10f;
    public float MoveSpeed => moveSpeed;

    [Header("Controle no Chão")]
    [SerializeField, Min(0f)] private float groundAcceleration = 75f;
    [SerializeField, Min(0f)] private float groundDeceleration = 95f;

    [Header("Controle no Ar")]
    [SerializeField, Min(0f)] private float airAcceleration = 50f;
    [SerializeField, Min(0f)] private float airDeceleration = 18f;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveHorizontal(float input, bool inAir = false)
    {
        float targetVelocityX = input * moveSpeed;
        Vector2 velocity = rb.linearVelocity;

        if (inAir && Mathf.Abs(velocity.x) > moveSpeed && Mathf.Sign(velocity.x) == Mathf.Sign(input))
        {
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, airDeceleration * Time.fixedDeltaTime);
            rb.linearVelocity = velocity;
            return;
        }

        float rate;
        if (inAir)
        {
            rate = (Mathf.Abs(input) > 0.01f) ? airAcceleration : airDeceleration;
        }
        else
        {
            rate = (Mathf.Abs(input) > 0.01f) ? groundAcceleration : groundDeceleration;
        }

        velocity.x = Mathf.MoveTowards(velocity.x, targetVelocityX, rate * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }

    public void StopHorizontal()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, 0f, groundDeceleration * Time.fixedDeltaTime);
        rb.linearVelocity = velocity;
    }
}