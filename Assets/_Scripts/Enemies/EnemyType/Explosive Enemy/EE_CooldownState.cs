/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EE_CooldownState : CooldownState
{
    private ExplosiveEnemy enemy;

    public EE_CooldownState(Entity entity, string animBoolName, D_CooldownState stateData, ExplosiveEnemy enemy) : base(entity, animBoolName, stateData)
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
                stateMachine.ChangeState(enemy.chargeState);
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
*/