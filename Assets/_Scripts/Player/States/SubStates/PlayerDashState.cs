using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerActionState
{
    private int amountOfJumpsLeft;

    public PlayerDashState(Player player, string animBoolName) : base(player, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter() {
		base.Enter();
		player.InputHandler.UseJumpInput();
		Movement?.SetVelocityY(playerData.jumpVelocity);
		isActionDone = true;
		amountOfJumpsLeft--;
	}

    public bool CanJump() {
		if (amountOfJumpsLeft > 0) 
		{
			return true;
		} else 
		{
			return false;
		}
	}

	public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

	public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    
}
