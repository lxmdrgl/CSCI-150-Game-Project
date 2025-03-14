using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class RR_ChargeState : ChargeState
{
    private RadialRanger enemy;

    public RR_ChargeState(Entity entity, string animBoolName, D_ChargeState stateData, RadialRanger enemy) : base(entity, animBoolName, stateData)
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

        //Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

        if (isPlayerInPursuitRange)
        {
            Vector3 playerPosition = enemy.targetPlayer.position;
            Vector3 enemyPosition = enemy.transform.position;

            float horizontalDistance = Mathf.Abs(playerPosition.x - enemyPosition.x);
            float verticalDistance = playerPosition.y - enemyPosition.y;

            Vector2 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;

            if ((direction.x > 0 &&  Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
            {
                Movement?.Flip();
            }

            // Normal attack if player is roughly at the same height and in front
            if (verticalDistance < 3f)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }
            // Radial attack if the player is higher up
            else if (verticalDistance >= 3f)
            {
                stateMachine.ChangeState(enemy.radialRangedAttackState);
            }

        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
