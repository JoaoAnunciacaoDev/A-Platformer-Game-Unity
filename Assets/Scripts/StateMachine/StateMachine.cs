using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State initialState;

    private State previousState;
    private State currentState;
    private Player player;

    private readonly Dictionary<string, State> states = new();

    public State PreviousState => previousState;
    public State CurrentState => currentState;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("StateMachine: Player não encontrado.", this);
            return;
        }

        foreach (State state in GetComponentsInChildren<State>(true))
        {
            states[state.gameObject.name.ToLowerInvariant()] = state;
            state.Initialize(player);
        }
    }

    private void Start()
    {
        if (initialState != null)
        {
            currentState = initialState;
            player.Visual.SetColor(currentState.DebugColor);
            currentState.OnStateEnter();
        }
        else
        {
            Debug.LogError("StateMachine: Initial State não foi definido.", this);
        }
    }

    private void Update()
    {
        if (currentState == null) return;

        State newState = currentState.OnInputHandle();

        if (newState != null)
            TransitionTo(newState);

        if (currentState != null)
        {
            newState = currentState.OnStateUpdate();

            if (newState != null)
                TransitionTo(newState);
        }
    }

    private void FixedUpdate()
    {
        if (currentState == null)
            return;

        State newState = currentState.OnStateFixedUpdate();

        if (newState != null)
            TransitionTo(newState);
    }

    public void TransitionTo(State newState)
    {
        if (newState == null || newState == currentState) return;

        currentState?.OnStateExit();

        previousState = currentState;
        currentState = newState;

        player.Visual.SetColor(currentState.DebugColor);
        currentState.OnStateEnter();
    }

    public State GetState(string stateName)
    {
        states.TryGetValue(stateName.ToLowerInvariant(), out State state);
        return state;
    }
}