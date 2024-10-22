using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
   public StateMachineBehaviour currentState {  get; private set; }

    public void Initialize(EnemyState startingState)
    {
        currentState = startingState;
    }
}
