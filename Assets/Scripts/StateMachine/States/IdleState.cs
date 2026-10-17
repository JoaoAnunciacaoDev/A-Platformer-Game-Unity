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
        player.PlayAnim("idle");
        player.ResetJumpCount();
        player.ResetDash();

        // player.SetVelocityX(0f, 0f);
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

        player.Move(Time.fixedDeltaTime, 0f);

        if (player.CanJump() && player.IsJumpBuffering())
        {
            player.ZeroJumpBuffer();
            return stateMachine.GetState("jump");
        }

        if (inputAxis != 0 && player.IsOnFloor())
            return stateMachine.GetState("walk");

        if (player.IsFalling())
            return stateMachine.GetState("fall");

        if (player.IsTouchingWall() && Input.GetButton("Climb"))
            return stateMachine.GetState("slide");

        if (player.HasDashCount() && player.IsDashBuffering())
            return stateMachine.GetState("dash");

        return null;
    }
}