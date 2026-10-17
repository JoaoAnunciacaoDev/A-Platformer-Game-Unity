using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected Player player;

    public virtual void Initialize(Player player) { this.player = player; }

    public virtual void OnStateEnter() { }

    public virtual void OnStateExit() { }

    public virtual State OnInputHandle() { return null; }

    public virtual State OnStateUpdate() { return null; }

    public virtual State OnStateFixedUpdate() { return null; }
}