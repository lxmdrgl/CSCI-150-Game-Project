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
        // // test Debug.Log("Entered Idle State");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState) 
        {
            if (xInput != 0) 
            {
                GameObject particle = GameObject.Instantiate(player.idleToMoveParticle, player.transform.position, Quaternion.identity);
                if (xInput < 0)
                    particle.transform.rotation = Quaternion.Euler(0, 180, 0);
                else
                    particle.transform.rotation = Quaternion.Euler(0, 0, 0);
                stateMachine.ChangeState(player.MoveState);
            }
        }
    }
}
