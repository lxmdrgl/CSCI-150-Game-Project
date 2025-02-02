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

    private CollisionSenses CollisionSenses
    {
        get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses);
    }

    private Movement movement;
    private CollisionSenses collisionSenses;

    protected int xInput;
    protected bool jumpInput;
    protected bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;

    protected Collider2D isPlatformOverlapBottom;
    protected Collider2D isPlatformOverlapTop;

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
            isPlatformOverlapBottom = CollisionSenses.PlatformOverlapBottom;
            isPlatformOverlapTop = CollisionSenses.PlatformOverlapTop;
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
        dashInput = player.InputHandler.DashInput;

        ResetPlatformCollision();

        // Debug.Log($"Actual y: {player.RB.linearVelocity.y}");

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
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        } 
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        } 
        else if (isTouchingWall && xInput == Movement?.FacingDirection) { // note: removed && Movement?.CurrentVelocity.y <= 0
            stateMachine.ChangeState(player.WallGrabState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);

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
        if (platformDropped != null && isPlatformOverlapBottom == null) {
            Debug.Log("Reset platform collision");
            Physics2D.IgnoreCollision(platformDropped, player.boxCollider, false);
            platformDropped = null;
        }
    }
}
