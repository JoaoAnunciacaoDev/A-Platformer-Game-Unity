
using UnityEngine;

public class JumpState : State
{
    private StateMachine stateMachine;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("jump");

        if (player.JumpController.VerticalVelocity <= 0.1f)
        {
            player.JumpController.Jump();
        }
    }

    public override State OnInputHandle()
    {
        if (player.InputHandler.JumpReleased)
            player.JumpController.CutJump();

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float input = player.InputHandler.Horizontal;

        player.Movement.MoveHorizontal(input, inAir: true);
        player.Visual.Flip(input);

        if (player.JumpController.VerticalVelocity <= 0f)
            return stateMachine.GetState("fall");

        return null;
    }
}
