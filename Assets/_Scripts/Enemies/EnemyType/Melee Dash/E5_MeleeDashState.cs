using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_MeleeDashState : MeleeAttackState
{
    private Enemy5 enemy;

    private float dashSpeed;
    private float dashDuration;
    private float jumpSpeed;
    private float dashStartTime;


    private bool triggeredAttack;

    public E5_MeleeDashState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Enemy5 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
    {
        this.enemy = enemy;

        dashSpeed = 15f;
        jumpSpeed = 15f;
        dashDuration = 0.5f;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        
    }

    public override void Enter()
    {
        base.Enter();
        triggeredAttack = false;

        dashStartTime = Time.time;
        Movement?.SetVelocityY(jumpSpeed); // Apply an upward force for jumping
        // Debug.LogWarning("Enemy 5 Y jump: " + + Movement?.CurrentVelocity.y);
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Debug.Log("Enemy 5 Y: " + Movement?.CurrentVelocity.y);

        if (!triggeredAttack) {
            // Debug.Log("Triggering attack in MeleeDashState: " + triggeredAttack);
            triggeredAttack = TriggerAttack();
        }

        Movement?.SetVelocityX(dashSpeed * Movement.FacingDirection);

        // If dash duration is over, transition to another state
        if (Time.time >= dashStartTime + dashDuration)
        {
            Movement.SetVelocityX(0);
            stateMachine.ChangeState(enemy.cooldownState); // Assuming IdleState exists
        }
    }

    
     public override void Exit()
    {
        base.Exit();

        Movement.SetVelocityX(0);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override bool TriggerAttack()
    {
        return base.TriggerAttack();
        // return false;
    }
}