
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
        // if (player.IsDead())
        //     return stateMachine.TransitionTo(stateMachine.GetState("death"));

        player.PlayAnim("fall");
    }

    public override State OnInputHandle()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!player.CoyoteTimerIsStopped())
                return stateMachine.GetState("jump");

            player.StartJumpBuffer();
        }

        if (Input.GetButtonDown("Dash"))
            player.StartDashBuffer();

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float inputAxis = Input.GetAxisRaw("Horizontal");

        player.Move(Time.fixedDeltaTime, inputAxis);
        player.FlipSprite(inputAxis);

        if (player.IsOnFloor())
        {
            player.PlaySquash();
            player.SpawnDustEffect();
            player.ResetDash();

            if (player.IsJumpBuffering())
                return stateMachine.GetState("jump");

            if (inputAxis == 0)
                return stateMachine.GetState("idle");

            return stateMachine.GetState("walk");
        }

        if (player.IsTouchingWall())
            return stateMachine.GetState("slide");

        if (player.HasDashCount() && player.IsDashBuffering())
            return stateMachine.GetState("dash");

        return null;
    }
}
