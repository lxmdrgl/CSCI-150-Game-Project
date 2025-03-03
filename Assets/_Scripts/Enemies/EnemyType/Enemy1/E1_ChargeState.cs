using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class E1_ChargeState : ChargeState
{
    private Enemy1 enemy;

    public E1_ChargeState(Entity entity, string animBoolName, D_ChargeState stateData, Enemy1 enemy) : base(entity, animBoolName, stateData)
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
        // Movement?.SetVelocityY(10.0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (!isPlayerInAgroRange && isPlayerInPursuitRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
