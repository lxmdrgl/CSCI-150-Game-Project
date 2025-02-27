using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RR_DeadState : DeadState
{
    private RadialRanger enemy;

    public RR_DeadState(Entity entity, string animBoolName, D_DeadState stateData, RadialRanger enemy) : base(entity, animBoolName, stateData)
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
