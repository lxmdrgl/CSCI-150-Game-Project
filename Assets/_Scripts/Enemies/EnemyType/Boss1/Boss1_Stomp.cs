using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Stomp : MeleeAttackState
{
    private Boss1 enemy;
	protected bool isAttackOffCooldown;

    public Boss1_Stomp(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Boss1 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
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
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            // Transition to another attack (combo) or cooldown
            if (entity.CheckPlayerInMaxAgroRange())
            {
                stateMachine.ChangeState(enemy.meleeAttackState); // Combo to overhead swing
            }
            else
            {
                stateMachine.ChangeState(enemy.cooldownState); // Go to cooldown
            }
        }
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