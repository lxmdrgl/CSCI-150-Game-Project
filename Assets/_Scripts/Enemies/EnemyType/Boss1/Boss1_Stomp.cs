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

            // Check if the player is in medium range for a melee attack
            if (distanceToPlayer > enemy.stompRange && distanceToPlayer <= enemy.swingRange && enemy.lastAttackType != Boss1.LastAttackType.Swing)
            {
                // test Debug.LogWarning("Transitioning to melee attack state from stomp attack state.");
                enemy.lastAttackType = Boss1.LastAttackType.Stomp;
                stateMachine.ChangeState(enemy.meleeAttackState); // Transition to melee
            }
            else
            {
                stateMachine.ChangeState(enemy.cooldownState); // Transition to cooldown
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
        enemy.StartCoroutine(enemy.SpawnSpikes());
        return false;
    }
}