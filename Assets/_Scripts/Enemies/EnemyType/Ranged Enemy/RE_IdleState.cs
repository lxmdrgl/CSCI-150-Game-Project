using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_IdleState : IdleState
{
    private RangedEnemy enemy;

    public RE_IdleState(Entity entity, string animBoolName, D_IdleState stateData, RangedEnemy enemy) : base(entity, animBoolName, stateData)
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

        if (isPlayerInAgroRange && isPlayerInPursuitRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
