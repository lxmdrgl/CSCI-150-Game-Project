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
    private bool fallInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private RaycastHit2D isPlatformBottom;
    private Collider2D platformDropped = null;
    private float delayAirTime = 0f;

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
            isPlatformBottom = CollisionSenses.PlatformBottom;
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
        fallInput = player.InputHandler.FallInput;

        /* if(!fallInput && (bool)isPlatformBottom)
        {
            platformDropped = isPlatformBottom.collider;
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, false);
        } */

        if (player.DashAttackState.CanAttackCooldown(CombatInputs.primaryAttackPress, CombatInputs.dashAttack))
        {
            stateMachine.ChangeState(player.DashAttackState);
        }
        else if (player.PrimaryAttackPressState.CanAttack() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && !player.PrimaryAttackHoldState.CanAttack())))
        {
            Debug.Log("Primary Attack Press state");
            stateMachine.ChangeState(player.PrimaryAttackPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && player.PrimaryAttackHoldState.CanAttack())
        {
            Debug.Log("Primary Attack Hold state");
            stateMachine.ChangeState(player.PrimaryAttackHoldState);
        }
        else if (player.SecondaryAttackPressState.CanAttack() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackHold] && !player.SecondaryAttackHoldState.CanAttack())))
        {
            Debug.Log("Secondary Attack Press state");
            stateMachine.ChangeState(player.SecondaryAttackPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondaryAttackHold] && player.SecondaryAttackHoldState.CanAttack())
        {
            Debug.Log("Secondary Attack Hold state");
            stateMachine.ChangeState(player.SecondaryAttackHoldState);
        }
        else if (player.PrimarySkillPressState.CanAttackCooldown() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillHold] && !player.PrimarySkillHoldState.CanAttackCooldown())))
        {
            Debug.Log("Primary Skill Press state");
            stateMachine.ChangeState(player.PrimarySkillPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillHold] && player.PrimarySkillHoldState.CanAttackCooldown())
        {
            Debug.Log("Primary Skill Hold state");
            stateMachine.ChangeState(player.PrimarySkillHoldState);
        }

        else if (player.SecondarySkillPressState.CanAttackCooldown() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillHold] && !player.SecondarySkillHoldState.CanAttackCooldown())))
        {
            Debug.Log("Secondary Skill Press state");
            stateMachine.ChangeState(player.SecondarySkillPressState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillHold] && player.SecondarySkillHoldState.CanAttackCooldown())
        {
            Debug.Log("Secondary Skill Hold state");
            stateMachine.ChangeState(player.SecondarySkillHoldState);
        }
        // if (player.InputHandler.AttackInputs[(int)CombatInputs.primarySkillPress] && player.PrimarySkillPressState.CanAttackCooldown())
        // {
        //     stateMachine.ChangeState(player.PrimarySkillPressState);
        // }
        // else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondarySkillPress] && player.SecondarySkillPressState.CanAttackCooldown())
        // {
        //     stateMachine.ChangeState(player.SecondarySkillPressState);
        // }
        else if (dashInput && player.DashState.CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (fallInput /* jumpInput && downInput */ && (bool)isPlatformBottom)
        {
            Debug.Log($"Jump Input: {jumpInput}, Down Input: {downInput}");
            player.InputHandler.UseJumpInput();
            // platformDropped = isPlatformDown;
            platformDropped = isPlatformBottom.collider;
            Debug.Log($"Current State Drop platform: {platformDropped}, {player.boxCollider}");
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, true);

            player.AirState.SetPlatformDropped(platformDropped);
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        } 
        /* else if (jumpInput && downInput && !(bool)isPlatformDown && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        } */
        else if (jumpInput /* && !downInput */ && player.JumpState.CanJump())
        {
            Debug.Log($"Jump Input: {jumpInput}, Down Input: {downInput}");
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (!isGrounded && DelayAirState(delayAirTime))
        {
            Debug.Log("Ground to Air state");
            player.AirState.StartCoyoteTime();
            stateMachine.ChangeState(player.AirState);
        } else {
        }
    }

    public bool DelayAirState(float delayTime)
    {
        if (Time.time >= startTime + delayTime)
        {
            delayTime = 0f;
            return true;
        }
        return false;
    }

    public void SetDelayTime(float delayTime) => delayAirTime = delayTime;

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
