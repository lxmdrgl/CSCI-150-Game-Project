using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_StunState : StunState
{
    private Enemy5 enemy;

    public E5_StunState(Entity entity, string animBoolName, D_StunState stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
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
      
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunTimeOver)
        {
            if (isPlayerInPursuitRange && isPlayerInAgroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else if(isPlayerInPursuitRange && !isPlayerInAgroRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
