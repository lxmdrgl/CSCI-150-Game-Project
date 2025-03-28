using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class StunState : EnemyState {
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_StunState stateData;

	protected bool isStunTimeOver;
	// protected bool isGrounded;
	// protected bool isMovementStopped;
	protected bool performCloseRangeAction;
	protected bool isPlayerInAgroRange;
	protected bool isPlayerInPursuitRange;

	public StunState(Entity entity, string animBoolName, D_StunState stateData) : base(entity, animBoolName) {
		this.stateData = stateData;
	}

	public override void DoChecks() {
		base.DoChecks();

		// isGrounded = CollisionSenses.Ground;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
		isPlayerInPursuitRange = entity.isPlayerInPursuitRange;

	}

	public override void Enter() {
		base.Enter();

		isStunTimeOver = false;
		// isMovementStopped = false;
		Movement?.SetVelocityX(0f);
	}

	public override void Exit() {
		base.Exit();
		entity.ResetStun();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (Time.time >= startTime + stateData.stunTime) {
			isStunTimeOver = true;
		}

		/* if (isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped) {
			isMovementStopped = true;
			Movement?.SetVelocityX(0f);
		} */
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
