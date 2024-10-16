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

    private int xInput;
    private bool jumpInput;

    private bool isGrounded;
    private bool isTouchingWall;

    private bool coyoteTime;

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
        }
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

        if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && xInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        } 
        else if (isGrounded && Movement?.CurrentVelocity.y < 0.01f && xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        } 
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        } 
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0) {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

}
