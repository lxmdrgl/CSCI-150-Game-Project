using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class PlayerActionState : PlayerState
{
    protected bool isActionDone;

	protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	private bool isGrounded;
	protected bool isTouchingWall;

    public PlayerActionState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks() {
		base.DoChecks();

		if (CollisionSenses) 
		{
			isGrounded = CollisionSenses.Ground;
			isTouchingWall = CollisionSenses.WallFront;
		}
	}

    public override void Enter() {
		base.Enter();

		isActionDone = false;
	}

    public override void LogicUpdate() {
		base.LogicUpdate();

		if (isActionDone) 
		{
			if (isGrounded && Movement?.CurrentVelocity.y < 0.01f) 
			{
				stateMachine.ChangeState(player.IdleState);
			} else 
			{
				stateMachine.ChangeState(player.AirState);
			}
		}
	}
}
