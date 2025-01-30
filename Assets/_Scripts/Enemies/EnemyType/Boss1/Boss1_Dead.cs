using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Dead : DeadState
{
    private Boss1 enemy;

    public Boss1_Dead(Entity entity, string animBoolName, D_DeadState stateData, Boss1 enemy) : base(entity, animBoolName, stateData)
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
