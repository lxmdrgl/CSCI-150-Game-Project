using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class E5_ChargeState : ChargeState
{
    private Enemy5 enemy;

    public E5_ChargeState(Entity entity, string animBoolName, D_ChargeState stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
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

        Debug.Log("Enemy Charge State: " + isPlayerInPursuitRange + " " + isPlayerInAgroRange + " " + performCloseRangeAction + " " + isDetectingLedge + " " + isDetectingWall);
        if (performCloseRangeAction)
        {
            Debug.Log("Perform close range action");
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (!isPlayerInAgroRange && isPlayerInPursuitRange)
        {
            Debug.Log("Enter look state");
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        // else if (!isPlayerInPursuitRange)
        // {
        //     Debug.Log("Enter idle state");
        //     stateMachine.ChangeState(enemy.idleState);
        // }
        else if (!isDetectingLedge || isDetectingWall)
        {
            Debug.Log("Enter look 2 state");
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
