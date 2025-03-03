using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_MoveState : MoveState
{
    private Enemy5 enemy;

    public E5_MoveState(Entity entity, string animBoolName, D_MoveState stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
