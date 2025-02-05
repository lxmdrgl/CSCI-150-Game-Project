using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class AttackState : EnemyState 
{

	private Movement movement;
	// private ParryReceiver parryReceiver;

	protected GameObject meleeAttackCollider;

	protected bool isAnimationFinished;
	protected bool isPlayerInAgroRange;
	protected bool isPlayerInPursuitRange;
	public AttackState(Entity entity, string animBoolName, GameObject meleeAttackCollider) : base(entity, animBoolName) 
	{
		this.meleeAttackCollider = meleeAttackCollider;

		movement = core.GetCoreComponent<Movement>();
		// parryReceiver = core.GetCoreComponent<ParryReceiver>();
	}

	public override void DoChecks() 
	{
		base.DoChecks();

		isPlayerInAgroRange = entity.CheckPlayerInAgroRange();
		isPlayerInPursuitRange = entity.CheckPlayerInPursuitRange();
	}

	public override void Enter() 
	{
		base.Enter();

		entity.atsm.attackState = this;
		isAnimationFinished = false;
		movement?.SetVelocityX(0f);
	}

	public override void Exit() 
	{
		base.Exit();
	}

	public override void LogicUpdate() 
	{
		base.LogicUpdate();
		movement?.SetVelocityX(0f);
		movement?.SetVelocityY(0f);
	}

	public override void PhysicsUpdate() 
	{
		base.PhysicsUpdate();
	}

	public virtual void TriggerAttack() 
	{
		
	}

	public virtual void FinishAttack() 
	{
		isAnimationFinished = true;
	}

	// public void SetParryWindowActive(bool value) => parryReceiver.SetParryColliderActive(value);
}
