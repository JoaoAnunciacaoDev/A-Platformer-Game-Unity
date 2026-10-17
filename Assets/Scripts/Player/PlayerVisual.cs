
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    public void Flip(float horizontalInput)
    {
        if (spriteRenderer == null || horizontalInput == 0f)
            return;

        spriteRenderer.flipX = horizontalInput < 0f;
    }

    public void PlayAnimation(string animationName)
    {
        if (animator == null)
            return;

        int stateHash = Animator.StringToHash(animationName);

        if (animator.HasState(0, stateHash))
            animator.Play(stateHash);
    }
}
