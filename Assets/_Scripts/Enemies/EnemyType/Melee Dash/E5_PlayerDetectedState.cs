using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_PlayerDetectedState : PlayerDetectedState
{
    private Enemy5 enemy;

    public E5_PlayerDetectedState(Entity entity, string animBoolName, D_PlayerDetected stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDetectedTimeOver) 
        {
            Debug.Log("Enemy Detected State: " + isPlayerInPursuitRange + " " + isPlayerInAgroRange + " " + isDetectingLedge);
            if (isPlayerInPursuitRange && isPlayerInAgroRange)
            {
                Debug.Log("Enter charge state");
                stateMachine.ChangeState(enemy.chargeState);
            }
            else if(!isPlayerInAgroRange && isPlayerInPursuitRange)
            {
                Debug.Log("Enter look state");
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
            else if (!isDetectingLedge)
            {
                Debug.Log("Enter move flip state");
                Movement?.Flip();
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                Debug.Log("Enter move state");
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
