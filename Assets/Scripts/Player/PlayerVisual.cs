using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    private Vector3 originalScale;
    private Vector3 originalLocalPosition;

    private void Awake()
    {
        if (spriteRenderer != null)
        {
            originalScale = spriteRenderer.transform.localScale;
            originalLocalPosition = spriteRenderer.transform.localPosition;
        }
    }

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

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }

    public void SetCrouchVisual(bool crouch)
    {
        if (spriteRenderer == null) return;

        if (crouch)
        {
            spriteRenderer.transform.localScale = new Vector3(originalScale.x * 1.3f, originalScale.y * 0.5f, originalScale.z);
            spriteRenderer.transform.localPosition = new Vector3(originalLocalPosition.x, -0.375f, originalLocalPosition.z);
        }
        else
        {
            spriteRenderer.transform.localScale = originalScale;
            spriteRenderer.transform.localPosition = originalLocalPosition;
        }
    }
}