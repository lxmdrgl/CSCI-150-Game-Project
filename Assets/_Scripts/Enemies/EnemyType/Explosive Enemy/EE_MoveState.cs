/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE_MoveState : MoveState
{
    private ExplosiveEnemy enemy;

    public EE_MoveState(Entity entity, string animBoolName, D_MoveState stateData, ExplosiveEnemy enemy) : base(entity, animBoolName, stateData)
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
*/