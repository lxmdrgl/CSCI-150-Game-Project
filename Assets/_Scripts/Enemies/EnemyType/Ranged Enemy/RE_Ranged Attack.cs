using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_RangedAttackState : RangedAttackState
{
    private RangedEnemy enemy;

    public RE_RangedAttackState(Entity entity, string animBoolName, Transform attackPosition, D_RangedAttackState stateData, RangedEnemy enemy) 
        : base(entity, animBoolName, attackPosition, stateData)
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
        
    }

    public override void Exit()
    {
        base.Exit();
        // Cleanup or reset anything when exiting this state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            // Use Entity methods for range checks
            if (entity.CheckPlayerInAgroRange()) // Player is in agro range
            {
                stateMachine.ChangeState(enemy.playerDetectedState); // Transition to PlayerDetectedState
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState); // Transition to LookForPlayerState
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
       
    }
}