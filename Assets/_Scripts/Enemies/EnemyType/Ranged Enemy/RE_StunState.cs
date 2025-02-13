using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE_StunState : StunState
{
    private RangedEnemy enemy;

    public RE_StunState(Entity entity, string animBoolName, D_StunState stateData, RangedEnemy enemy) : base(entity, animBoolName, stateData)
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
