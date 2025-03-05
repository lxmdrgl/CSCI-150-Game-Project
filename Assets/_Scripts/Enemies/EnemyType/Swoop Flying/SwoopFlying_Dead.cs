using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopFlying_Dead : DeadState
{
    private SwoopFlying enemy;

    public SwoopFlying_Dead(Entity entity, string animBoolName, D_DeadState stateData, SwoopFlying enemy) : base(entity, animBoolName, stateData)
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
