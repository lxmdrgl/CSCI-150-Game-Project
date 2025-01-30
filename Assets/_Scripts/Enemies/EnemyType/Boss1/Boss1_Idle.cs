using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Idle : IdleState
{
    private Boss1 enemy;

    public Boss1_Idle(Entity entity, string animBoolName, D_IdleState stateData, Boss1 enemy) : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
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

        if (isPlayerInPursuitRange)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
