using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerWallState
{
    private Vector2 holdPosition;

    public PlayerWallGrabState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

	public override void Enter() {
		base.Enter();

		holdPosition = player.transform.position;

		HoldPosition();

		// Wall Jump? Ground Slam?
	}

    public override void LogicUpdate()
    {
        base.LogicUpdate();

		if (!isExitingState)
		{
			HoldPosition();
		}
    }

    private void HoldPosition() {
		player.transform.position = holdPosition;

		Movement?.SetVelocityX(0f);
		Movement?.SetVelocityY(0f);
	}
}
