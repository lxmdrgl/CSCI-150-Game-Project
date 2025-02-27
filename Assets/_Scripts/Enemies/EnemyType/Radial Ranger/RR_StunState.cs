using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RR_StunState : StunState
{
    private RadialRanger enemy;

    public RR_StunState(Entity entity, string animBoolName, D_StunState stateData, RadialRanger enemy) : base(entity, animBoolName, stateData)
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
        enemy.meleeAttackState.DisableHitbox();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.meleeAttackState.DisableHitbox();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isStunTimeOver)
        {
            if (isPlayerInPursuitRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
