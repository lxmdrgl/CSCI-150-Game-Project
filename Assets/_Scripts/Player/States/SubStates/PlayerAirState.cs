using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
public class PlayerAirState : PlayerState
{
    protected Movement Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    protected CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private Movement movement;
    protected CollisionSenses collisionSenses;

    protected int xInput;
    protected bool jumpInput;
    protected bool downInput;
    protected bool dashInput;

    private bool isGrounded;
    private bool isCloseToGrounded;
    private bool isTouchingWall;

    protected Collider2D isPlatformOverlap;
    protected Collider2D isPlatformOverlapTop;
    // protected RaycastHit2D isPlatformTop;
    // protected RaycastHit2D isPlatformBottomUp;
    protected RaycastHit2D isPlatformBottomExtend;

    private bool coyoteTime;

    private Collider2D platformDropped;

    public PlayerAirState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isCloseToGrounded = CollisionSenses.LongGround;

            isPlatformOverlap = CollisionSenses.PlatformOverlap;
            isPlatformOverlapTop = CollisionSenses.PlatformOverlapTop;
            // isPlatformTop = CollisionSenses.PlatformTop;
            // isPlatformBottomUp = CollisionSenses.PlatformBottomUp;
            isPlatformBottomExtend = CollisionSenses.PlatformBottomExtend;
        }
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();

        isTouchingWall = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        downInput = player.InputHandler.DownInput;
        dashInput = player.InputHandler.DashInput;

        ResetPlatformCollision();

        // Debug.Log($"Actual y: {player.RB.linearVelocity.y}");

        if (downInput && player.FallAttackState.CanAttack(CombatInputs.primaryAttackPress, CombatInputs.fallAttack))
        {
            stateMachine.ChangeState(player.FallAttackState);
        }
        else if (player.DashAttackState.CanAttackCooldown(CombatInputs.primaryAttackPress, CombatInputs.dashAttack))
        {
            stateMachine.ChangeState(player.DashAttackState);
        }
        else if (player.PrimaryAttackState.CanAttack() && 
                (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackPress]
                || (player.InputHandler.AttackInputs[(int)CombatInputs.primaryAttackHold] && !player.PrimaryAttackHoldState.CanAttack())))
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
        else if (jumpInput && player.JumpState.CanJump())
        {
            Debug.Log("Input: Air to Jump");
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (isGrounded && platformDropped == null && Movement?.CurrentVelocity.y < 0.01f && xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        } 
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        } 
        else if (isTouchingWall && xInput == Movement?.FacingDirection && !isCloseToGrounded 
                 && Movement?.CurrentVelocity.y <= 0 && !downInput) { 
            stateMachine.ChangeState(player.WallGrabState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);

            if (Movement.CurrentVelocity.y < playerData.terminalVelocityY)
            {
                Movement?.SetVelocityY(playerData.terminalVelocityY);
            }

            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        }
    }

    protected void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void SetPlatformDropped(Collider2D platform)
    {
        platformDropped = platform;
    }   

    public void ResetPlatformCollision()
    {
        // Debug.Log("Reset platform : " + (bool)platformDropped + ", " + (bool)isPlatformTop);
        // isPlatformTop checks whether the the platform is above the top of the player
        // isPlatformBottomUp checks whether the the platform is 
        if (platformDropped != null && !isPlatformOverlap) {
            Debug.Log("Reset platform Success: " + (bool)platformDropped + ", " + (bool)!isPlatformOverlap);
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, false);
            platformDropped = null;
        }
    }
}
