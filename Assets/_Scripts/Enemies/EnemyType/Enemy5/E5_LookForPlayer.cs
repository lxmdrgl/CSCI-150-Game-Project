using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E5_LookForPlayerState : LookForPlayerState
{
    private Enemy5 enemy;

    public E5_LookForPlayerState(Entity entity, string animBoolName, D_LookForPlayer stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
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

        if(isPlayerInAgroRange && isPlayerInPursuitRange) 
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
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
