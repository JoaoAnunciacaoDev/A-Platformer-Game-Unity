using UnityEngine;

public class Player : MonoBehaviour
{
    public void PlayAnim(string animationName)
    {
    }

    public void ResetJumpCount()
    {
    }

    public void ResetDash()
    {
    }

    public void StartJumpBuffer()
    {
    }

    public void StartDashBuffer()
    {
    }

    public void Move(float delta, float verticalInput)
    {
    }

    public bool CanJump()
    {
        return false;
    }

    public bool IsJumpBuffering()
    {
        return false;
    }

    public void ZeroJumpBuffer()
    {
    }

    public bool IsOnFloor()
    {
        return false;
    }

    public bool IsFalling()
    {
        return false;
    }

    public bool IsTouchingWall()
    {
        return false;
    }

    public bool HasDashCount()
    {
        return false;
    }

    public bool IsDashBuffering()
    {
        return false;
    }
}