using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_DeadState : DeadState
{
    private Enemy5 enemy;

    public E5_DeadState(Entity entity, string animBoolName, D_DeadState stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
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
