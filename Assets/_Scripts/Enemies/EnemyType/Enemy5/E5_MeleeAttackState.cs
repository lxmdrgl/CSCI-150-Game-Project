using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_MeleeAttackState : MeleeAttackState
{
    private Enemy5 enemy;
	protected bool isAttackOffCooldown;

    protected bool triggeredAttack = false;

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
            stateMachine.ChangeState(enemy.cooldownState);
        }

        // Loop attack for the entire state
        /* if (!triggeredAttack) {
            TriggerAttack();
            triggeredAttack = true;
        } */

        // Needs a time
        /* if (Time.time >= startTime + dashTime)
        {
            isAttackOver = true;
        } */
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
