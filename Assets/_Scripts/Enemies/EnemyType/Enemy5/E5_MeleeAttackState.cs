using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_MeleeAttackState : MeleeAttackState
{
    private Enemy5 enemy;
	protected bool isAttackOffCooldown;

    protected bool triggeredAttack = false;

     public float dashTime = 0.2f;
     public float dashSpeed = 10f;


    public E5_MeleeAttackState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Enemy5 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
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
        Movement?.SetVelocityX(dashSpeed * Movement.FacingDirection); // Apply dash movement
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(dashSpeed * Movement.FacingDirection); // Apply dash movement

        if (Time.time >= startTime + dashTime)
        {
            stateMachine.ChangeState(enemy.idleState); // Assuming IdleState exists
        }

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.cooldownState);
        }


        // Loop attack for the entire state
         if (!triggeredAttack) {
            triggeredAttack = TriggerAttack();
        } 

        // Needs a time
        /* if (Time.time >= startTime + dashTime)
        {
            isAttackOver = true;
        } */
    }

    
     public override void Exit()
    {
        base.Exit();
        Movement?.SetVelocityX( 0);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override bool TriggerAttack()
    {
        base.TriggerAttack();
        return false;
    }
}