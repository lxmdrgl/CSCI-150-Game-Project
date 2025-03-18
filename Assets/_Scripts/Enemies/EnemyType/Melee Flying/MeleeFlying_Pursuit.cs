using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class MeleeFlying_Pursuit : ChargeState
{
    private MeleeFlying enemy;

    public MeleeFlying_Pursuit(Entity entity, string animBoolName, D_ChargeState stateData, MeleeFlying enemy) : base(entity, animBoolName, stateData)
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
            Movement.SetVelocityX(0);
            Movement.SetVelocityY(0);
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if(isPlayerInPursuitRange)
        {
            Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;

            if ((direction.x > 0 &&  Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
            {
                Movement.Flip();
            }

            Movement.SetVelocityX(direction.x * stateData.chargeSpeed);
            Movement.SetVelocityY(direction.y * stateData.chargeSpeed);
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
