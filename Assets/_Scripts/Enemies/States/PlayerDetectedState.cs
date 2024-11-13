using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class PlayerDetectedState : EnemyState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

    protected D_PlayerDetected stateData;
    // protected Enemy1 enemy;  // Reference to the specific enemy type

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool performCloseRangeAction;

    public PlayerDetectedState(Entity entity, string animBoolName, D_PlayerDetected stateData) : base(entity, animBoolName)
    {
        this.stateData = stateData;
        // this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isDetectingLedge = CollisionSenses.LedgeVertical;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {

        base.Enter();
        Movement?.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(0f);

        /* if (!isPlayerInMaxAgroRange)
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
        } */
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
