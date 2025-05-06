using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, string animBoolName) : base(player, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);
        // Debug.Log("Entered Idle State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState) 
        {
            if (xInput != 0) 
            {
                GameObject particle = GameObject.Instantiate(player.idleToMoveParticle, player.transform.position, Quaternion.identity);
                particle.transform.rotation = player.transform.rotation;
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
