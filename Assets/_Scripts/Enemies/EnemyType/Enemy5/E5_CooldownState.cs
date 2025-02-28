using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_CooldownState : CooldownState
{
    private Enemy5 enemy;

    public E1_CooldownState(Entity entity, string animBoolName, D_CooldownState stateData, Enemy5 enemy) : base(entity, animBoolName, stateData)
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

        if(isCooldownTimeOver)
        {
            if (isPlayerInPursuitRange && isPlayerInAgroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else if (isPlayerInPursuitRange && !isPlayerInAgroRange)
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
            else
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    
}
