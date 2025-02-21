using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_MeleeAttackState : MeleeAttackState
{
    private RangedEnemy enemy;
	protected bool isAttackOffCooldown;

    public RE_MeleeAttackState(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, RangedEnemy enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
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
