using UnityEngine;

public class DiveState : State
{
    [Header("Velocidades do Dive")]
    [SerializeField, Min(0f)] private float diagonalSpeedX = 14f;
    [SerializeField, Min(0f)] private float diagonalSpeedY = 18f;
    [SerializeField, Min(0f)] private float verticalDiveSpeed = 22f;

    [Header("Windup")]
    [SerializeField, Min(0f)] private float neutralWindupTime = 0.18f;
    private StateMachine stateMachine;
    private float lockedDirX;
    private float windupTimer;
    private bool isHanging;
    public float LockedDirectionX => lockedDirX;
    
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("dive");
        Rigidbody2D rb = player.Rb;
        float inputX = player.InputHandler.Horizontal;

        float jumpDir = player.JumpController.LastJumpDirectionX;
        if (jumpDir == 0f && Mathf.Abs(rb.linearVelocity.x) > 0.5f)
        {
            jumpDir = Mathf.Sign(rb.linearVelocity.x);
        }

        bool isOppositeToJump = (jumpDir != 0f) 
                                && (Mathf.Abs(inputX) > 0.2f) 
                                && (Mathf.Sign(inputX) != jumpDir);

        if (Mathf.Abs(inputX) > 0.2f && !isOppositeToJump)
        {
            lockedDirX = Mathf.Sign(inputX);
            isHanging = false;
            rb.linearVelocity = new Vector2(lockedDirX * diagonalSpeedX, -diagonalSpeedY);
            player.Visual.Flip(lockedDirX);
        }
        else
        {
            lockedDirX = 0f;
            isHanging = true;
            windupTimer = neutralWindupTime;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public override State OnStateFixedUpdate()
    {
        Rigidbody2D rb = player.Rb;
        if (isHanging)
        {
            rb.linearVelocity = Vector2.zero;
            windupTimer -= Time.fixedDeltaTime;
            if (windupTimer <= 0f)
            {
                isHanging = false;
                rb.linearVelocity = new Vector2(0f, -verticalDiveSpeed);
            }
            return null;
        }

        Vector2 vel = rb.linearVelocity;
        vel.x = (lockedDirX != 0f) ? (lockedDirX * diagonalSpeedX) : 0f;
        rb.linearVelocity = vel;

        if (player.Collision.IsGrounded)
        {
            Vector2 v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;

            if (lockedDirX != 0f && player.InputHandler.IsDownHeld)
            {
                return stateMachine.GetState("slide");
            }
            if (lockedDirX != 0f)
                return stateMachine.GetState("walk");
            return stateMachine.GetState("idle");
        }

        return null;
    }
}