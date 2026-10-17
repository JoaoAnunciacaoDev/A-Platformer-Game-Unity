using UnityEngine;

public class SlideState : State
{
    [Header("Configurações do Slide")]
    [Tooltip("Velocidade inicial ao começar a deslizar")]
    [SerializeField, Min(0f)] private float initialSlideSpeed = 18f;

    [Tooltip("Taxa de desaceleração natural")]
    [SerializeField, Min(0f)] private float slideDeceleration = 20f;
    [SerializeField, Min(0f)] private float minSlideDuration = 0.15f;

    private StateMachine stateMachine;
    private float slideDirection;
    private float currentSpeed;
    private float slideTimer;

    public override void Initialize(Player player)
    {
        base.Initialize(player);
        stateMachine = GetComponentInParent<StateMachine>();
    }

    public override void OnStateEnter()
    {
        player.Visual.PlayAnimation("slide");
        player.Collision.SetCrouchCollider(true);

        slideTimer = 0f;

        Rigidbody2D rb = player.Rb;
        float inputX = player.InputHandler.Horizontal;

        if (Mathf.Abs(inputX) > 0.01f)
            slideDirection = Mathf.Sign(inputX);
        else if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
            slideDirection = Mathf.Sign(rb.linearVelocity.x);
        else
            slideDirection = player.Visual.transform.localScale.x >= 0 ? 1f : -1f;

        currentSpeed = initialSlideSpeed;
        player.Visual.Flip(slideDirection);

        Vector2 vel = rb.linearVelocity;
        vel.y = 0f;
        vel.x = slideDirection * currentSpeed;
        rb.linearVelocity = vel;
    }

    public override void OnStateExit()
    {
        player.Collision.SetCrouchCollider(false);
    }

    public override State OnInputHandle()
    {
        if (player.InputHandler.JumpPressed || player.JumpController.HasBufferedJump)
        {
            player.JumpController.SlideJump(slideDirection);
            return stateMachine.GetState("jump");
        }

        return null;
    }

    public override State OnStateFixedUpdate()
    {
        Rigidbody2D rb = player.Rb;
        slideTimer += Time.fixedDeltaTime;

        float inputX = player.InputHandler.Horizontal;
        bool holdingSlideDirection = (Mathf.Abs(inputX) > 0.01f && Mathf.Sign(inputX) == slideDirection);
        bool holdingOppositeDirection = (Mathf.Abs(inputX) > 0.01f && Mathf.Sign(inputX) == -slideDirection);

        float decelRate = holdingOppositeDirection ? (slideDeceleration * 2.5f) : slideDeceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, decelRate * Time.fixedDeltaTime);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = slideDirection * currentSpeed;
        rb.linearVelocity = velocity;

        if (!player.Collision.IsGrounded && slideTimer > 0.05f)
        {
            return stateMachine.GetState("fall");
        }

        if (player.Collision.IsTouchingWall && player.Collision.WallDirection == (int)slideDirection)
        {
            return stateMachine.GetState("idle");
        }

        if (!player.Collision.HasCeilingAbove && slideTimer >= minSlideDuration)
        {
            if (holdingSlideDirection)
            {
                if (!player.InputHandler.IsDownHeld || currentSpeed <= player.Movement.MoveSpeed)
                {
                    return stateMachine.GetState("walk");
                }
            }
            else if (holdingOppositeDirection && currentSpeed <= 1f)
            {
                return stateMachine.GetState("walk");
            }
            else if (!holdingSlideDirection && (!player.InputHandler.IsDownHeld || currentSpeed <= 0.5f))
            {
                if (Mathf.Abs(inputX) > 0.01f)
                    return stateMachine.GetState("walk");

                return stateMachine.GetState("idle");
            }
        }

        return null;
    }
}