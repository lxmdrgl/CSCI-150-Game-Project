using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopFlying_Cooldown : CooldownState
{
    private SwoopFlying enemy;

    public SwoopFlying_Cooldown(Entity entity, string animBoolName, D_CooldownState stateData, SwoopFlying enemy) : base(entity, animBoolName, stateData)
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
            if (isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
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
