using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_DeadState : DeadState
{
    private RangedEnemy enemy;

    public RE_DeadState(Entity entity, string animBoolName, D_DeadState stateData, RangedEnemy enemy) : base(entity, animBoolName, stateData)
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
