using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class ChargeState : EnemyState
{

    public Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;


	protected D_ChargeState stateData;

	protected bool isPlayerInAgroRange;
	protected bool isPlayerInPursuitRange;
	protected bool isDetectingLedge;
	protected bool isDetectingWall;
	protected bool isChargeTimeOver;
	protected bool performCloseRangeAction;

    public ChargeState(Entity entity, string animBoolName, D_ChargeState stateData) : base(entity, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks() {
		base.DoChecks();

		isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
		isPlayerInPursuitRange = entity.CheckPlayerInAgroRange();
		isDetectingLedge = CollisionSenses.LedgeVertical;
		isDetectingWall = CollisionSenses.WallFront;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
	}


    public override void Enter() 
	{
		base.Enter();
	}

	public override void Exit() 
	{
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		Movement?.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
