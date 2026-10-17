
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    private InputActionMap playerMap;
    private InputAction moveAction;
    private InputAction jumpAction;

    public float Horizontal =>
        moveAction.ReadValue<Vector2>().x;

    public bool JumpPressed =>
        jumpAction.WasPressedThisFrame();

    public bool JumpReleased =>
        jumpAction.WasReleasedThisFrame();

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player", true);
        moveAction = playerMap.FindAction("Move", true);
        jumpAction = playerMap.FindAction("Jump", true);
    }

    private void OnEnable()
    {
        playerMap?.Enable();
    }

    private void OnDisable()
    {
        playerMap?.Disable();
    }
}
