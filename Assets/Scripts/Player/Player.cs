
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerCollision))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerVisual))]
public class Player : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerJump JumpController { get; private set; }
    public PlayerCollision Collision { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerVisual Visual { get; private set; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        Movement = GetComponent<PlayerMovement>();
        JumpController = GetComponent<PlayerJump>();
        Collision = GetComponent<PlayerCollision>();
        InputHandler = GetComponent<PlayerInputHandler>();
        Visual = GetComponent<PlayerVisual>();
    }
}
