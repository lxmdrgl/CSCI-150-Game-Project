using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class PlayerWallState : PlayerState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

    protected bool isGrounded;
	protected bool isTouchingWall;
    protected bool jumpInput;
    protected bool downInput;
    protected bool dashInput;
    protected int xInput;
	protected int yInput;
    
    public PlayerWallState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses) 
        {
			isGrounded = CollisionSenses.Ground;
			isTouchingWall = CollisionSenses.WallFront;
        }
    }

    public override void Enter() 
    {
		base.Enter();
	}

	public override void Exit() 
    {
		base.Exit();
	}

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
		yInput = player.InputHandler.NormInputY;
        jumpInput = player.InputHandler.JumpInput;
        downInput = player.InputHandler.DownInput;
        dashInput = player.InputHandler.DashInput;

        if (player.PrimaryAttackPressState.CanAttack() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && !player.PrimaryAttackHoldState.CanAttack())))
        {
            Movement?.CheckIfShouldFlip(-Movement.FacingDirection);
            stateMachine.ChangeState(player.PrimaryAttackPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && player.PrimaryAttackHoldState.CanAttack())
        {
            Movement?.CheckIfShouldFlip(-Movement.FacingDirection);
            stateMachine.ChangeState(player.PrimaryAttackHoldState);
        }
        else if (player.SecondaryAttackPressState.CanAttack() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackHold] && !player.SecondaryAttackHoldState.CanAttack())))
        {
            Movement?.CheckIfShouldFlip(-Movement.FacingDirection);
            stateMachine.ChangeState(player.SecondaryAttackPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackHold] && player.SecondaryAttackHoldState.CanAttack())
        {
            Movement?.CheckIfShouldFlip(-Movement.FacingDirection);
            stateMachine.ChangeState(player.SecondaryAttackHoldState);
        }
        else if (dashInput && player.DashState.CanDash())
        {
            Movement?.CheckIfShouldFlip(-Movement.FacingDirection);
            stateMachine.ChangeState(player.DashState);
        }
        else if (jumpInput) 
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (isGrounded)
        {
            stateMachine.ChangeState(player.IdleState);
        }  
        else if (!isTouchingWall || downInput)
        {
            stateMachine.ChangeState(player.AirState);
        } 
    }

    public override void PhysicsUpdate() 
    {
		base.PhysicsUpdate();
	}
}
