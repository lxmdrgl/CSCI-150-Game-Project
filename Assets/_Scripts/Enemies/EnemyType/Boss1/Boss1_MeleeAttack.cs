using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_MeleeAttack : MeleeAttackState
{
    private Boss1 enemy;
	protected bool isAttackOffCooldown;
    public Boss1_MeleeAttack(Entity entity, string animBoolName, GameObject meleeAttackCollider, D_MeleeAttack stateData, Boss1 enemy) : base(entity, animBoolName, meleeAttackCollider, stateData)
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
        Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;
        if ((direction.x > 0 && Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement.Flip();
        }
        isAttackOffCooldown = false; // Reset cooldown flag
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
            float distanceToPlayer = Vector2.Distance(enemy.targetPlayer.position, enemy.transform.position);

            if (distanceToPlayer <= enemy.stompRange && enemy.lastAttackType != Boss1.LastAttackType.Stomp)
            {
                enemy.lastAttackType = Boss1.LastAttackType.Swing;
                stateMachine.ChangeState(enemy.stompAttackState); 
            }
            else
            {
                enemy.lastAttackType = Boss1.LastAttackType.Swing;
                stateMachine.ChangeState(enemy.cooldownState); 
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
        enemy.SpawnStalactites(); 
        return false;
    }
}
