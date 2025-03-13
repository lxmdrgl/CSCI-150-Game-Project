using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using UnityEditor;
using System;
public class PlayerPlatformAirState : PlayerAirState
{
	private Vector2 platformBottomPosition;
	private bool jumpInputBuffer = false;

    public PlayerPlatformAirState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }	
	public override void Enter() {
		base.Enter();
	}

	public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate() {
		// base.LogicUpdate();

		CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
		jumpInput = player.InputHandler.JumpInput;
		dashInput = player.InputHandler.DashInput;

        ResetPlatformCollision();

		Movement?.CheckIfShouldFlip(xInput);

		Movement?.SetVelocityX(playerData.platformMovementVelocity * xInput);
		Movement?.SetVelocityY(playerData.platformVelocity);
		// Debug.Log($"set y: {Movement?.CurrentVelocity.y}");

		// Debug.Log($"Platform State: isPlatformOverlap: {isPlatformOverlap != null}, isPlatformOverlapTop: {isPlatformOverlapTop != null}");

		if (!isExitingState) 
		{
			// Debug.Log($"isPlatformOverlap: {isPlatformOverlap}, isPlatformOverlapTop: {isPlatformOverlapTop}");
			if (isPlatformOverlap != null || isPlatformOverlapTop != null)
			{	
				if (downInput && player.FallAttackState.CanAttack(CombatInputs.primaryAttackPress, CombatInputs.fallAttack))
				{
					// StopPlatformMove();
					stateMachine.ChangeState(player.FallAttackState);
				}
				else if (player.DashAttackState.CanAttackCooldown(CombatInputs.primaryAttackPress, CombatInputs.dashAttack))
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.DashAttackState);
				}
				else if (player.PrimaryAttackState.CanAttack() && 
						(player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackPress]
						|| (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && !player.PrimaryAttackHoldState.CanAttack())))
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.PrimaryAttackState);
				}
				else if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && player.PrimaryAttackHoldState.CanAttack())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.PrimaryAttackHoldState);
				}
				else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackPress] && player.SecondaryAttackState.CanAttack())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.SecondaryAttackState);
				}
				if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillPress] && player.PrimarySkillState.CanAttackCooldown())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.PrimarySkillState);
				}
				else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillPress] && player.SecondarySkillState.CanAttackCooldown())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.SecondarySkillState);
				}
				else if (dashInput && player.DashState.CanDash())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.DashState);
				}
				else if (jumpInput && player.JumpState.CanJump())
				{
					/* StopPlatformMove();
					player.AirState.SetJumpingInPlatform(true);
					stateMachine.ChangeState(player.JumpState); */
					jumpInputBuffer = true;
				} 
			}
			else if (isPlatformOverlap == null && isPlatformOverlapTop == null) 
			{
				MovePlatformPosition();
				StopPlatformMove();
				isExitingState = true;

				if (jumpInputBuffer) 
				{
					jumpInputBuffer = false;
					Debug.Log("move platform jump buffer");
					player.AirState.SetJumpingInPlatform(true);
					stateMachine.ChangeState(player.JumpState);
				} 
				else if(xInput == 0)
				{
					player.IdleState.SetDelayTime(0.1f);
					stateMachine.ChangeState(player.IdleState);
				} 
				else if (xInput != 0) 
				{
					stateMachine.ChangeState(player.MoveState);
				}
			} else {
				Debug.Log("Platform State: No condition met");
			}
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}

	public void StopPlatformMove() 
	{
		Debug.Log("stop platform move");
		Movement?.SetVelocityY(0);
	}

	private void MovePlatformPosition() 
	{
		Debug.Log("move platform position");
		Movement?.SetVelocityY(0);
		isPlatformBottomExtend = CollisionSenses.PlatformBottomExtend;
		if (isPlatformBottomExtend.collider != null) {
			platformBottomPosition = new Vector2(player.transform.position.x, player.transform.position.y - isPlatformBottomExtend.distance +0.25f);
			Debug.Log($"Move platform position: {platformBottomPosition.y}, {player.transform.position.y}, {isPlatformBottomExtend.distance}");
			player.transform.position = platformBottomPosition;
		}
	}
}
