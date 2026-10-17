
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
        player.PlayAnim("jump");
        player.SpawnDustEffect();
        player.Jump();
        player.StopCoyoteTimer();
        player.EmitJumped();

        SoundController.PlaySFX("jump");
    }

    public override State OnInputHandle()
    {
        if (Input.GetButtonUp("Jump"))
        {
            player.CutVelocity();
            return stateMachine.GetState("fall");
        }

        if (Input.GetButtonDown("Dash"))
        {
            player.StartDashBuffer();
        }

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        float inputAxis = Input.GetAxisRaw("Horizontal");

        player.Move(Time.fixedDeltaTime, inputAxis);
        player.FlipSprite(inputAxis);

        if (player.IsFalling())
            return stateMachine.GetState("fall");

        // if (player.IsTouchingWall())
        //     return stateMachine.GetState("slide");

        if (player.HasDashCount() && player.IsDashBuffering())
            return stateMachine.GetState("dash");

        return null;
    }
}
