
using UnityEngine;

public class IdleState : State
{
    private StateMachine stateMachine;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("idle");
        player.Movement.StopHorizontal();
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
        if (!player.Collision.IsGrounded)
            return stateMachine.GetState("fall");

        if (!Mathf.Approximately(player.InputHandler.Horizontal, 0f))
            return stateMachine.GetState("walk");

        player.Movement.StopHorizontal();

        return null;
    }
}
