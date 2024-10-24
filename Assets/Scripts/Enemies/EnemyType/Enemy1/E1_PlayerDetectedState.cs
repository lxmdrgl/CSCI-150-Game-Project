using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_PlayerDetectedState : PlayerDetectedState
{
    private Enemy1 enemy1;

    public E1_PlayerDetectedState(Enemy1 enemy, EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemy1 = enemy;
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

        if (!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy1.idleState);
        }
        else if (!isDetectingLedge)
        {
            enemy1.Flip();
            stateMachine.ChangeState(enemy1.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
