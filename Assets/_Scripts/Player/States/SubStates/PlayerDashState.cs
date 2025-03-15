using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.CoreSystem;
using UnityEngine;

public class PlayerDashState : PlayerActionState
{
	// protected DamageReceiver DamageReceiver { get => damageReceiver ?? core.GetCoreComponent(ref damageReceiver); }
	// private DamageReceiver damageReceiver;
	// protected KnockBackReceiver KnockBackReceiver { get => knockBackReceiver ?? core.GetCoreComponent(ref knockBackReceiver); }
	// private KnockBackReceiver knockBackReceiver;
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
		Debug.Log("Enter dash");
		DamageReceiver?.SetCanTakeDamage(false);
		KnockBackReceiver?.SetCanTakeKnockBack(false);

		player.dashTimeNotifier.Disable();
		player.dashAttackTimeNotifier.Disable();
		dashEnabled = false;
	}

    public override void Exit()
    {
        base.Exit();

		Debug.Log("Exit dash");
		DamageReceiver?.SetCanTakeDamage(true);
		KnockBackReceiver?.SetCanTakeKnockBack(true);
		player.dashTimeNotifier.Init(playerData.dashCooldown);
		player.dashAttackTimeNotifier.Init(playerData.dashAttackCooldown);
		player.DashAttackState.DashAttackCooldownEnable();
		Debug.Log("Init dash attack timer");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
		// Debug.Log($"jump: {jumpInput} attack: {attackInputs.Any(x => x)}");
        if (Time.time >= startTime + playerData.dashTime || jumpInput || attackInputs.Any(x => x)) {
			Debug.Log("End dash: " + Time.time + " " + (startTime + playerData.dashTime) + " " + jumpInput + " " + attackInputs.Any(x => x));
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
