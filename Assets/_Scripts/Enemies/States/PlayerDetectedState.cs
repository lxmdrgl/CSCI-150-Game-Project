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

    protected bool isPlayerInAgroRange;
    protected bool isPlayerInPursuitRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool performCloseRangeAction;

    protected bool isDetectedTimeOver;

    protected float detectedTime;

    public PlayerDetectedState(Entity entity, string animBoolName, D_PlayerDetected stateData) : base(entity, animBoolName)
    {
        this.stateData = stateData;
        // this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isDetectingLedge = CollisionSenses.LedgeVertical;
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInPursuitRange = entity.isPlayerInPursuitRange;

    }

    public override void Enter()
    {

        base.Enter();
        Movement?.SetVelocityX(0f);
        isDetectedTimeOver = false;
        detectedTime = stateData.detectedTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.SetVelocityX(0f);

        if (Time.time >= startTime + detectedTime)
        {
            isDetectedTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
