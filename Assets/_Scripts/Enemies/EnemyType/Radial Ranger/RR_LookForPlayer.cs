using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RR_LookForPlayerState : LookForPlayerState
{
    private RadialRanger enemy;

    public RR_LookForPlayerState(Entity entity, string animBoolName, D_LookForPlayer stateData, RadialRanger enemy) : base(entity, animBoolName, stateData)
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

        if(isPlayerInPursuitRange) 
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
