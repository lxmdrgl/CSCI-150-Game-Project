using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class E5_ChargeState : ChargeState
{
    private Enemy5 enemy;
    private float lastDashTime;
    private bool canDash;
    private float dashCooldown = 2f;  // Time between dashes
    private float dashForce = 20f;    // Dash speed multiplier

    public E5_ChargeState(Entity entity, string animBoolName, D_ChargeState stateData, Enemy5 enemy) 
        : base(entity, animBoolName, stateData)
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
        lastDashTime = Time.time - dashCooldown; // Allow instant dash on entering state
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Regular charge movement
        Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

        // Dash Mechanic: If cooldown has passed, perform a dash
        if (Time.time >= lastDashTime + dashCooldown)
        {
            PerformDash();
            lastDashTime = Time.time;
        }

        // State Transitions
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

    private void PerformDash()
    {
        Debug.Log("Enemy is dashing!");  // Debug message to check dashing behavior
        Movement?.SetVelocityX(dashForce * Movement.FacingDirection);
    }
}
