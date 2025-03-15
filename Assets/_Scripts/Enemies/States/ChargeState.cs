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
	protected bool isPlayerInMaxAgroRange;
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
		isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
		isDetectingLedge = CollisionSenses.LedgeVertical;
		isDetectingWall = CollisionSenses.WallFront;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInPursuitRange = entity.isPlayerInPursuitRange;

	}


    public override void Enter() 
	{
		base.Enter();

		int randomPlayerIndex = Random.Range(1, 3);

		if(randomPlayerIndex==1 && entity.playerPosition1 != null)
		{
			entity.targetPlayer = entity.playerPosition1;
		}
		else if(randomPlayerIndex==2 && entity.playerPosition2 != null)
		{
			entity.targetPlayer = entity.playerPosition2;
		}
		else if(entity.playerPosition1 != null) // rolled a 2 and no player 2
		{
			entity.targetPlayer = entity.playerPosition1;
		}
	}

	public override void Exit() 
	{
		base.Exit();
		
	}

	public override void LogicUpdate() 
	{
		base.LogicUpdate();
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
