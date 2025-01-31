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
    private bool downJumpInput;
    private bool dashInput;
    private bool isGrounded;
    private bool isTouchingWall;

    private bool isOnPlatform;
    private Collider2D platformDropped = null;
    private Collider2D platformCollider = null;
    private bool platformOverlap;

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
            isOnPlatform = CollisionSenses.Platform;
            platformCollider = CollisionSenses.PlatformCollider;
            // platformOverlap = CollisionSenses.PlatformOverlap;
            
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
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
        downJumpInput = player.InputHandler.DownJumpInput;
        dashInput = player.InputHandler.DashInput;
        
        if (platformDropped != null && !platformOverlap) {
            Debug.Log("Reset platform collision");
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, false);
            platformDropped = null;
        }

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttack] && player.PrimaryAttackState.CanAttack())
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttack] && player.SecondaryAttackState.CanAttack())
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkill] && player.PrimarySkillState.CanAttackCooldown())
        {
            stateMachine.ChangeState(player.PrimarySkillState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkill] && player.SecondarySkillState.CanAttackCooldown())
        {
            stateMachine.ChangeState(player.SecondarySkillState);
        }
        else if (dashInput && player.DashState.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (downJumpInput && isOnPlatform)
        {
            Debug.Log($"{CollisionSenses.PlatformCollider}, {player.boxCollider}");
            Physics2D.IgnoreCollision(platformCollider, player.boxCollider, true);
            platformDropped = platformCollider;
        } 
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (!isGrounded)
        {
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
