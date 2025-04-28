using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class Boss1_Pursuit : ChargeState
{
    private Boss1 enemy;

    public Boss1_Pursuit(Entity entity, string animBoolName, D_ChargeState stateData, Boss1 enemy) : base(entity, animBoolName, stateData)
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float distanceToPlayer = Vector2.Distance(enemy.targetPlayer.position, enemy.transform.position);
        Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;

        // Check if the boss needs to flip to face the player
        if ((direction.x > 0 && Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement.Flip();
        }

        if (distanceToPlayer <= enemy.stompRange)
        {
            // Check if the player is close enough for a stomp attack
            Movement.SetVelocityX(0);
            stateMachine.ChangeState(enemy.stompAttackState); // Transition to stomp attack
        }
        else if (distanceToPlayer <= enemy.swingRange && distanceToPlayer > enemy.stompRange)
        {
            // Check if the player is close enough for a melee attack
            Movement.SetVelocityX(0);
            stateMachine.ChangeState(enemy.meleeAttackState); // Transition to melee attack
        }
        else if (isPlayerInPursuitRange)
        {
            Movement.SetVelocityX(direction.x * stateData.chargeSpeed);
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState); // Transition to idle state
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}