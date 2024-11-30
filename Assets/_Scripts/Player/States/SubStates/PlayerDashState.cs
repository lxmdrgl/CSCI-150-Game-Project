using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDashState : PlayerActionState
{
	protected int xInput;
	protected bool dashEnabled = true;
    public PlayerDashState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    public override void Enter() {
		base.Enter();
		player.InputHandler.UseDashInput();
		
        xInput = player.InputHandler.NormInputX;
		Movement?.SetVelocityX(xInput * playerData.dashVelocity);
		// Debug.Log("Enter dash");

		player.dashTimeNotifier.Disable();
		dashEnabled = false;
	}

    public override void Exit()
    {
        base.Exit();

		player.dashTimeNotifier.Init(playerData.dashCooldown);
		// Debug.Log("Init dash timer");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
		// Debug.Log($"jump: {jumpInput} attack: {attackInputs.Any(x => x)}");
        if (Time.time >= startTime + playerData.dashTime || jumpInput || attackInputs.Any(x => x)) {
			// Debug.Log("End dash");
			isActionDone = true;
		} 
		else if (xInput != 0) {
            Movement?.SetVelocityX(xInput * playerData.dashVelocity);
			// Debug.Log("Loop input dash: " + xInput * playerData.dashVelocity);
        } else {
			Movement?.SetVelocityX(Movement.FacingDirection * playerData.dashVelocity);
			// Debug.Log("Loop facing dash: " + Movement.FacingDirection * playerData.dashVelocity);
		}
    }

	public void ResetDashCooldown() => dashEnabled = true;

	public bool CanDash() => dashEnabled;

	/* public bool CanDash()  {
		// Debug.Log("try can dash: " + dashEnabled);

		return dashEnabled;
	} */
}
