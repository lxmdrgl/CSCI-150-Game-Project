using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class LookForPlayerState : EnemyState {
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_LookForPlayer stateData;

	protected bool turnImmediately;
	protected bool isPlayerInAgroRange;
	protected bool isPlayerInPursuitRange;
	protected bool isAllTurnsDone;
	protected bool isAllTurnsTimeDone;

	protected float lastTurnTime;

	protected int amountOfTurnsDone;

	public LookForPlayerState(Entity entity, string animBoolName, D_LookForPlayer stateData) : base(entity, animBoolName) {
		this.stateData = stateData;
	}

	public override void DoChecks() 
	{
		base.DoChecks();

		isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
		isPlayerInPursuitRange = entity.CheckPlayerInPursuitRange();
	}

	public override void Enter() {
		base.Enter();

		isAllTurnsDone = false;
		isAllTurnsTimeDone = false;

		lastTurnTime = startTime;
		amountOfTurnsDone = 0;

		Movement?.SetVelocityX(0f);
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		Movement?.SetVelocityX(0f);

		if (turnImmediately) {
			Movement?.Flip();
			lastTurnTime = Time.time;
			amountOfTurnsDone++;
			turnImmediately = false;
		} else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone) {
			Movement?.Flip();
			lastTurnTime = Time.time;
			amountOfTurnsDone++;
		}

		if (amountOfTurnsDone >= stateData.amountOfTurns) {
			isAllTurnsDone = true;
		}

		if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone) {
			isAllTurnsTimeDone = true;
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}

	public void SetTurnImmediately(bool flip) {
		turnImmediately = flip;
	}
}

