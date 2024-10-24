using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : EnemyState
{
    protected D_PlayerDetected stateData;
    protected Enemy1 enemy;  // Reference to the specific enemy type

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isDetectingLedge;

    public PlayerDetectedState(Enemy1 enemy, EnemyStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = enemy.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = enemy.CheckPlayerInMaxAgroRange();
        isDetectingLedge = enemy.CheckLedge();
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isPlayerInMaxAgroRange)
        {
            // Transition to idle state
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInMinAgroRange)
        {
            // Change to move state to pursue the player
            stateMachine.ChangeState(enemy.moveState);
        }
        else
        {
            // Keep pursuing if ledge is not detected
            if (!isDetectingLedge)
            {
                enemy.Flip();
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                enemy.SetVelocity(stateData.movementSpeed);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
