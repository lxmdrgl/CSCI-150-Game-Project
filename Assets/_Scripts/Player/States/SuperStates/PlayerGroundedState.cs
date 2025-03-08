using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected int yInput;

    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private Movement movement;
    private CollisionSenses collisionSenses;

    private bool jumpInput;
    private bool downInput;
    private bool dashInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private RaycastHit2D isPlatformDown;
    private Collider2D platformDropped = null;

    public PlayerGroundedState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            // isPlatformDown = CollisionSenses.PlatformDown;
            isPlatformDown = CollisionSenses.PlatformBottom;
            // Debug.Log($"isPlatformDown: {isPlatformDown.collider}");
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.AirState.SetJumpingInPlatform(false);
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

        if (player.DashAttackState.CanDashAttackCooldown(CombatInputs.primaryAttackPress, CombatInputs.dashAttack))
        {
            stateMachine.ChangeState(player.DashAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackPress] && player.PrimaryAttackState.CanAttack()
            || (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && !player.PrimaryAttackHoldState.CanAttack()))
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && player.PrimaryAttackHoldState.CanAttack())
        {
            stateMachine.ChangeState(player.PrimaryAttackHoldState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackPress] && player.SecondaryAttackState.CanAttack())
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillPress] && player.PrimarySkillState.CanAttackCooldown())
        {
            stateMachine.ChangeState(player.PrimarySkillState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillPress] && player.SecondarySkillState.CanAttackCooldown())
        {
            stateMachine.ChangeState(player.SecondarySkillState);
        }
        else if (dashInput && player.DashState.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (jumpInput && downInput && (bool)isPlatformDown)
        {
            Debug.Log($"Jump Input: {jumpInput}, Down Input: {downInput}");
            player.InputHandler.UseJumpInput();
            // platformDropped = isPlatformDown;
            platformDropped = isPlatformDown.collider;
            Debug.Log($"Drop platform: {platformDropped}, {player.boxCollider}");
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, true);

            player.AirState.SetPlatformDropped(platformDropped);
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        } 
        else if (jumpInput && downInput && !(bool)isPlatformDown && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (jumpInput && !downInput && player.JumpState.CanJump())
        {
            Debug.Log($"Jump Input: {jumpInput}, Down Input: {downInput}");
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (!isGrounded)
        {
            Debug.Log("Ground to Air state");
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        } else {
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
