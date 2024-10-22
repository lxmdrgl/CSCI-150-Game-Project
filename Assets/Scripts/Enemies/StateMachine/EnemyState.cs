using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Entity entity;

    protected float startTime;

    public EnemyState(Entity etity, EnemyStateMachine stateMachine)
    {
        this.entity = etity;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }
}
