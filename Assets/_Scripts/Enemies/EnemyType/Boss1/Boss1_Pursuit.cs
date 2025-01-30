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
        
        if (performCloseRangeAction)
        {
            enemy.rb.linearVelocity = Vector2.zero;
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if(isPlayerInPursuitRange)
        {
            Vector2 direction = (enemy.player.position - enemy.transform.position).normalized;
            direction.y = enemy.transform.position.y;
            if ((direction.x > 0 &&  Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
            {
                Movement?.Flip();
            }

            enemy.rb.linearVelocityX = direction.x * stateData.chargeSpeed;
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
