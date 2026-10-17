
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
        player.PlayAnim("walk");
        player.ResetJumpCount();
        player.ResetDash();
    }

    public override State OnInputHandle()
    {
        if (Input.GetButtonDown("Jump"))
            player.StartJumpBuffer();

        if (Input.GetButtonDown("Dash"))
            player.StartDashBuffer();

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float inputAxis = Input.GetAxisRaw("Horizontal");

        player.Move(Time.fixedDeltaTime, inputAxis);
        player.FlipSprite(inputAxis);

        if (player.CanJump() && player.IsJumpBuffering())
        {
            player.ZeroJumpBuffer();
            return stateMachine.GetState("jump");
        }

        if (inputAxis == 0f && player.IsOnFloor())
            return stateMachine.GetState("idle");

        if (player.IsFalling())
            return stateMachine.GetState("fall");

        if (player.IsTouchingWall() && Input.GetButton("Climb"))
            return stateMachine.GetState("slide");

        if (player.HasDashCount() && player.IsDashBuffering())
            return stateMachine.GetState("dash");

        return null;
    }
}
