using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem;
using UnityEngine;

public class PlayerKnockBackState : PlayerActionState
{
    private bool knockBackActive;
    // private KnockBackReceiver KnockBackReceiver { get => knockBackReceiver ?? core.GetCoreComponent(ref knockBackReceiver); }
    // private KnockBackReceiver knockBackReceiver;
    public PlayerKnockBackState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        knockBackActive = false;
        KnockBackReceiver.OnKnockBackInactive += HandleKnockBackInactive;
        DamageReceiver?.SetCanTakeDamage(false);
        KnockBackReceiver?.SetCanTakeKnockBack(false);
    }

    public override void Exit()
    {
        base.Exit();

        DamageReceiver?.SetCanTakeDamage(true);
		KnockBackReceiver?.SetCanTakeKnockBack(true);
    }

    private void HandleKnockBackInactive()
    {
        knockBackActive = true;
        // Debug.Log("handle inactive true");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (knockBackActive) 
        {
            // Debug.Log("knock action done");
            isActionDone = true;
        }
    }
}
