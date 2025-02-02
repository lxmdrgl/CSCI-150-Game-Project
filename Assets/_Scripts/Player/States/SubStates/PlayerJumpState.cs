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
	}

    public bool CanJump() {
		Debug.Log("jumps left: " + amountOfJumpsLeft);
		if (amountOfJumpsLeft > 0) 
		{
			return true;
		} else 
		{
			return false;
		}
	}

	public void ResetAmountOfJumpsLeft() {
		Debug.Log("ResetAmountOfJumpsLeft");
		amountOfJumpsLeft = playerData.amountOfJumps;
	}

	public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    
}
