using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerActionState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, string animBoolName) : base(player, animBoolName)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter() {
		base.Enter();
		player.InputHandler.UseJumpInput();
		Movement?.SetVelocityY(playerData.jumpVelocity);
		isActionDone = true;
		amountOfJumpsLeft--;
		// test Debug.Log("jumps left: " + amountOfJumpsLeft);
	}

    public bool CanJump() {
		// test Debug.Log("jumps left: " + amountOfJumpsLeft);
		if (amountOfJumpsLeft > 0) 
		{
			return true;
		} else 
		{
			return false;
		}
	}

	public void ResetAmountOfJumpsLeft() {
		// test Debug.Log("ResetAmountOfJumpsLeft");
		amountOfJumpsLeft = playerData.amountOfJumps;
	}

	public void DecreaseAmountOfJumpsLeft() {
		// test Debug.Log("DecreaseAmountOfJumpsLeft");
		amountOfJumpsLeft--;
	}
    
}
