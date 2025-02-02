using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerWallJumpState : PlayerActionState
{
    private int wallJumpDirection;
    
    private int xInput;

    public PlayerWallJumpState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        player.JumpState.ResetAmountOfJumpsLeft();
        player.AirState.SetJumpingInPlatform(false);
        player.JumpState.DecreaseAmountOfJumpsLeft();

        wallJumpDirection = -Movement.FacingDirection;

        Movement?.SetVelocityY(playerData.wallJumpVelocityY);
        Movement?.CheckIfShouldFlip(wallJumpDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);

        if ((Time.time >= startTime + playerData.wallJumpTime) || (isTouchingWall && Time.time >= startTime + 0.05)) {
			isActionDone = true;
		}
        if(!isTouchingWall) {
            Movement?.SetVelocityX(Mathf.Clamp((playerData.movementVelocity * xInput) + (playerData.wallJumpVelocityX * wallJumpDirection),
                                               -playerData.wallJumpVelocityXMax ,playerData.wallJumpVelocityXMax));
        }
    }
}
