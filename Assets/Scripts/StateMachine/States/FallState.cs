
using UnityEngine;

public class FallState : State
{
    private StateMachine stateMachine;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("fall");
    }

    public override State OnInputHandle()
    {
        if (player.InputHandler.DownPressed)
        {
            return stateMachine.GetState("dive");
        }

        if (player.JumpController.CanJump)
        {
            return stateMachine.GetState("jump");
        }

        if (player.JumpController.CanWallJump)
        {
            float input = player.InputHandler.Horizontal;
            player.JumpController.WallJump(player.JumpController.LastWallDirection, input);
            return stateMachine.GetState("jump");
        }

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float input = player.InputHandler.Horizontal;

        player.Movement.MoveHorizontal(input, inAir: true);
        player.Visual.Flip(input);

        if (player.Collision.IsTouchingWall && !player.Collision.IsGrounded && player.JumpController.VerticalVelocity <= 0f)
        {
            if (input == 0 || Mathf.Sign(input) == player.Collision.WallDirection)
            {
                return stateMachine.GetState("wallslide");
            }
        }

        if (!player.Collision.IsGrounded)
            return null;

        if (player.JumpController.HasBufferedJump)
            return stateMachine.GetState("jump");

        if (Mathf.Approximately(input, 0f))
            return stateMachine.GetState("idle");

        return stateMachine.GetState("walk");
    }
}
