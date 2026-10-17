
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    private InputActionMap playerMap;
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction jumpAction;

    public float Horizontal =>
    (rightAction.ReadValue<float>() - leftAction.ReadValue<float>());

    public bool JumpPressed =>
        jumpAction.WasPressedThisFrame();

    public bool JumpReleased =>
        jumpAction.WasReleasedThisFrame();

    private void Awake()
    {
        playerMap = inputActions.FindActionMap("Player", true);
        leftAction = playerMap.FindAction("left", true);
        rightAction = playerMap.FindAction("right", true);
        jumpAction = playerMap.FindAction("jump", true);
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
