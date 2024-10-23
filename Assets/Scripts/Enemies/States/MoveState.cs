using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : EnemyState
{
    protected D_MoveState stateData;

   public MoveState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {

        this.stateData = stateData;

    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
