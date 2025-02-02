using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using Unity.VisualScripting;
public class PlayerNormalAirState : PlayerAirState
{

    private bool isJumpingInPlatform = false;

    public PlayerNormalAirState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }

    /* public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    } */

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState) 
        {
            if (isPlatformOverlapTop != null && Movement.CurrentVelocity.y > 0.01f && !isJumpingInPlatform) 
            {
                stateMachine.ChangeState(player.PlatformAirState);
            }
        }
    }

    public void SetJumpingInPlatform(bool value) => isJumpingInPlatform = value;


}
