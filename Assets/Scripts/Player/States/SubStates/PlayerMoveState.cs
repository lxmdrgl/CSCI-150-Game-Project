using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }	

    public override void LogicUpdate() {
		base.LogicUpdate();

		Movement?.CheckIfShouldFlip(xInput);

		Movement?.SetVelocityX(playerData.movementVelocity * xInput);

		if (!isExitingState) 
		{
			if (xInput == 0) 
			{
				stateMachine.ChangeState(player.IdleState);
			} 
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
