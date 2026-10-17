
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
        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float input = player.InputHandler.Horizontal;

        player.Movement.MoveHorizontal(input);
        player.Visual.Flip(input);

        if (!player.Collision.IsGrounded)
            return null;

        if (Mathf.Approximately(input, 0f))
            return stateMachine.GetState("idle");

        return stateMachine.GetState("walk");
    }
}
