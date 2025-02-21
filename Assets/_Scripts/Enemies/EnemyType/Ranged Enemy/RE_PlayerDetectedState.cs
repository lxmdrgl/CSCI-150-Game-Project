using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_PlayerDetectedState : PlayerDetectedState
{
    private RangedEnemy enemy;

    public RE_PlayerDetectedState(Entity entity, string animBoolName, D_PlayerDetected stateData, RangedEnemy enemy) : base(entity, animBoolName, stateData)
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

        if (isDetectedTimeOver) 
        {
            if (isPlayerInPursuitRange && isPlayerInAgroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else if(!isPlayerInAgroRange && isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
            else if (!isDetectingLedge)
            {
                Movement?.Flip();
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
