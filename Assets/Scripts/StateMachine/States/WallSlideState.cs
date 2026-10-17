using UnityEngine;

public class WallSlideState : State
{
    [Header("Settings")]
    [SerializeField, Min(0f)] private float slideSpeed = 3.5f;

    private StateMachine stateMachine;
    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("wall_slide");
        player.Visual.Flip(player.Collision.WallDirection);
    }

    public override State OnInputHandle()
    {
        if (player.JumpController.HasBufferedJump)
        {
            float input = player.InputHandler.Horizontal;
            player.JumpController.WallJump(player.Collision.WallDirection, input);
            return stateMachine.GetState("jump");
        }

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        if (player.Collision.IsGrounded)
        {
            return stateMachine.GetState("idle");
        }

        if (!player.Collision.IsTouchingWall)
        {
            return stateMachine.GetState("fall");
        }

        float input = player.InputHandler.Horizontal;
        if (input != 0 && Mathf.Sign(input) != player.Collision.WallDirection)
        {
            return stateMachine.GetState("fall");
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.linearVelocity;
        
        velocity.x = player.Collision.WallDirection * 0.5f;
        if (velocity.y < -slideSpeed)
        {
            velocity.y = -slideSpeed;
        }
        rb.linearVelocity = velocity;

        return null;
    }
}