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

	protected DamageReceiver DamageReceiver { get => damageReceiver ?? core.GetCoreComponent(ref damageReceiver); }
	private DamageReceiver damageReceiver;
	protected KnockBackReceiver KnockBackReceiver { get => knockBackReceiver ?? core.GetCoreComponent(ref knockBackReceiver); }
	private KnockBackReceiver knockBackReceiver;

	protected bool isGrounded;
	protected bool isTouchingWall;
	protected Collider2D isPlatformOverlap;
	protected bool jumpInput;
	protected bool dashInput;
	protected bool[] attackInputs;

    public PlayerActionState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks() {
		base.DoChecks();

		if (CollisionSenses) 
		{
			isGrounded = CollisionSenses.Ground;
			isTouchingWall = CollisionSenses.WallFront;
			isPlatformOverlap = CollisionSenses.PlatformOverlap;
		}
	}

    public override void Enter() {
		base.Enter();

		isActionDone = false;
	}

    public override void LogicUpdate() {
		base.LogicUpdate();

		jumpInput = player.InputHandler.JumpInput;
        dashInput = player.InputHandler.DashInput;
		attackInputs = player.InputHandler.AttackInputs;

		if (isActionDone) 
		{
			if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && isPlatformOverlap == null) 
			{
				stateMachine.ChangeState(player.IdleState);
			} else 
			{
				stateMachine.ChangeState(player.AirState);
			}
		}
	}
}
