using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class SwoopFlying_Pursuit : ChargeState
{
    private SwoopFlying enemy;
    private Vector2 targetPosition;
    private bool movingToTargetPosition;
    //private float stayTimer;
    //private const float stayDuration = 0.2f;

    public SwoopFlying_Pursuit(Entity entity, string animBoolName, D_ChargeState stateData, SwoopFlying enemy) : base(entity, animBoolName, stateData)
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
        movingToTargetPosition = true;
    }

    public override void Exit()
    {
        base.Exit();
        movingToTargetPosition = false;

        if (((enemy.player.position - enemy.transform.position).x > 0 && Movement?.FacingDirection < 0) || ((enemy.player.position - enemy.transform.position).x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CalculateTargetPosition();

        if (isPlayerInPursuitRange && movingToTargetPosition)
        {
            MoveToTargetPosition();
        }
        else if (!movingToTargetPosition)
        {
            stateMachine.ChangeState(enemy.cooldownState);
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

    private void CalculateTargetPosition()
    {
        // Calculate the top left or top right position relative to the player
        float offsetX = 5.0f; 
        float offsetY = 3.0f; 

        if (enemy.transform.position.x < enemy.player.position.x)
        {
            // Move to the top left of the player
            targetPosition = new Vector2(enemy.player.position.x - offsetX, enemy.player.position.y + offsetY);
        }
        else
        {
            // Move to the top right of the player
            targetPosition = new Vector2(enemy.player.position.x + offsetX, enemy.player.position.y + offsetY);
        }
    }

    private void MoveToTargetPosition()
    {
        Vector2 direction = (targetPosition - (Vector2)enemy.transform.position).normalized;

        if ((direction.x > 0 && Movement?.FacingDirection < 0) || (direction.x < 0 && Movement?.FacingDirection > 0))
        {
            Movement?.Flip();
        }

        enemy.rb.linearVelocity = direction * stateData.chargeSpeed;

        // Check if the enemy has reached the target position
        if (Vector2.Distance(enemy.transform.position, targetPosition) < 0.1f)
        {
            enemy.rb.linearVelocity = Vector2.zero;
            movingToTargetPosition = false;
        }

        
    }
}
