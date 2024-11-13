using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startingState) 
    {
        CurrentState = startingState;
        CurrentState.Enter();        
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        Debug.Log($"Current State: {CurrentState}");
        CurrentState.Enter();
    }
}
