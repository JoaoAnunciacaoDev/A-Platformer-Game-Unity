using UnityEngine;

public class DiveState : State
{
    [Header("Velocidades do Dive")]
    [SerializeField, Min(0f)] private float diagonalSpeedX = 14f;
    [SerializeField, Min(0f)] private float diagonalSpeedY = 18f;
    [SerializeField, Min(0f)] private float verticalDiveSpeed = 22f;

    private StateMachine stateMachine;
    private float lockedDirX;

    public float LockedDirectionX => lockedDirX;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("dive");

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        float inputX = player.InputHandler.Horizontal;

        if (Mathf.Abs(inputX) > 0.01f)
        {
            lockedDirX = Mathf.Sign(inputX);
        }
        else if (Mathf.Abs(rb.linearVelocity.x) > 1f)
        {
            lockedDirX = Mathf.Sign(rb.linearVelocity.x);
        }
        else
        {
            lockedDirX = 0f;
        }

        Vector2 diveVelocity;
        if (lockedDirX != 0f)
        {
            diveVelocity = new Vector2(lockedDirX * diagonalSpeedX, -diagonalSpeedY);
            player.Visual.Flip(lockedDirX);
        }
        else
        {
            diveVelocity = new Vector2(0f, -verticalDiveSpeed);
        }

        rb.linearVelocity = diveVelocity;
    }

    public override State OnInputHandle()
    {
        return null;
    }

    public override State OnStateFixedUpdate()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector2 vel = rb.linearVelocity;

        if (lockedDirX != 0f)
        {
            vel.x = lockedDirX * diagonalSpeedX;
        }
        else
        {
            vel.x = 0f;
        }

        rb.linearVelocity = vel;

        if (player.Collision.IsTouchingWall && !player.Collision.IsGrounded)
        {
            return stateMachine.GetState("wallslide");
        }

        if (player.Collision.IsGrounded)
        {
            if (lockedDirX != 0f)
            {
                return stateMachine.GetState("walk");
            }
            
            return stateMachine.GetState("idle");
        }

        return null;
    }
}