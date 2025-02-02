using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using UnityEditor;
public class PlayerPlatformAirState : PlayerAirState
{
	// private float platformTopPosition;

    public PlayerPlatformAirState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }	
	public override void Enter() {
		base.Enter();
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
		Debug.Log($"set y: {Movement?.CurrentVelocity.y}");

		if (!isExitingState) 
		{
			if (isPlatformOverlapTop != null)
			{	
				if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttack] && player.PrimaryAttackState.CanAttack())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.PrimaryAttackState);
				}
				else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttack] && player.SecondaryAttackState.CanAttack())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.SecondaryAttackState);
				}
				if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkill] && player.PrimarySkillState.CanAttackCooldown())
				{
					StopPlatformMove();
					stateMachine.ChangeState(player.PrimarySkillState);
				}
				else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkill] && player.SecondarySkillState.CanAttackCooldown())
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
					player.AirState.SetJumpingInPlatform(true);
					stateMachine.ChangeState(player.JumpState);
				} 
			}
			else if (isPlatformOverlapTop == null) 
			{
				Movement?.SetVelocityY(0);
				Debug.Log($"current y: {Movement?.CurrentVelocity.y}");

				stateMachine.ChangeState(player.AirState);
				/* if (xInput == 0) 
				{
					stateMachine.ChangeState(player.IdleState);
				}
				else if (xInput != 0) 
				{
					stateMachine.ChangeState(player.MoveState);
				} */
        	}
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}

	public void StopPlatformMove() {
		Movement?.SetVelocityY(0);
		Debug.Log($"current y: {Movement?.CurrentVelocity.y}");
	}
}
