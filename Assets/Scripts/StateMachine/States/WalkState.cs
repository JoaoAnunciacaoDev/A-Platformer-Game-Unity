
using UnityEngine;

public class WalkState : State
{
    private StateMachine stateMachine;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("walk");
    }

    public override State OnInputHandle()
    {
        if (player.InputHandler.JumpPressed &&
            player.Collision.IsGrounded)
        {
            return stateMachine.GetState("jump");
        }

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float input = player.InputHandler.Horizontal;

        player.Movement.MoveHorizontal(input);
        player.Visual.Flip(input);

        if (!player.Collision.IsGrounded)
            return stateMachine.GetState("fall");

        if (Mathf.Approximately(input, 0f))
            return stateMachine.GetState("idle");

        return null;
    }
}
