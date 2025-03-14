using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Weapons.Components;
using Game.CoreSystem;

public class EnemyDashState : MeleeAttackState
{
    private Enemy5 enemy;
    private float dashSpeed;
    private float dashDuration;
    private float dashStartTime;

	protected bool isDetectingLedge;
	protected bool isDetectingWall;
	protected bool isChargeTimeOver;
	protected bool performCloseRangeAction;

    /* public EnemyDashState(Entity entity, string animBoolName, float dashSpeed, float dashDuration) 
        : base(entity, animBoolName)
    {
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
    } */

    public EnemyDashState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Enemy5 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        dashStartTime = Time.time;

        // Apply an immediate force or velocity for dashing
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement?.SetVelocityX(dashSpeed * Movement.FacingDirection);

        // If dash duration is over, transition to another state
        if (Time.time >= dashStartTime + dashDuration)
        {
            stateMachine.ChangeState(enemy.idleState); // Assuming IdleState exists
        }
    }

    public override void Exit()
    {
        base.Exit();

        // Stop the dash by setting velocity to zero
        Movement.SetVelocityX(0);
    }
}
